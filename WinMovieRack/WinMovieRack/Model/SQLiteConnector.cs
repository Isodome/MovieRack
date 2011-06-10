using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace WinMovieRack.Model
{
    public class SQLiteConnector
    {
        private object DBProtect = new object();
        private SQLiteConnection connection;
        private HashSet<uint> imdbPersonIds;

        public void initDb()
        {
            string dataSource = "MovieRack.db";
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + dataSource;
            connection.Open();
            this.checkTables();

            imdbPersonIds = new HashSet<uint>();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "select imdbID FROM Person";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                this.imdbPersonIds.Add(uint.Parse(reader[0].ToString()));
            }

            createFolders();
        }

        private void createFolders()
        {
            Directory.CreateDirectory("img\\mov");
            Directory.CreateDirectory("img\\per");
        }

        public void checkTables()
        {
            FileInfo file = new FileInfo("createscript.sql");
            string script = file.OpenText().ReadToEnd();

            SQLiteCommand command = new SQLiteCommand(connection);


            // Erstellen der Tabelle, sofern diese noch nicht existiert.
            command.CommandText = script;
            executeCommandThreadSafe(command);

            // Einfügen eines Test-Datensatzes.
            command.CommandText = "INSERT INTO Movies (idMovies, year, title, plot, runtime, posterPath) VALUES(NULL, 1970, 'James Bond xx', 'laaangweilig', 120, 'Z:\\MovieRack\\WinMovieRack\\WinMovieRack\\Images\\#9.jpg')";
            //executeCommandThreadSafe(command);

            command.CommandText = "SELECT Plot, Title FROM Movies WHERE Year = 1970";

            SQLiteDataReader reader = executeReaderThreadSafe(command);

            while (reader.Read())
            {
                Console.WriteLine("Dies ist der {0}. eingefügte Datensatz mit dem Wert: \"{1}\"", reader[0].ToString(), reader[1].ToString());
            }
            // Freigabe der Ressourcen.
            command.Dispose();

        }

        public List<MRListData> getMovieList(MovieEnum editable)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> movieList = new List<MRListData>();
            command.CommandText = "SELECT idMovies, title, year, " + editable.ToString() + ", posterPath FROM Movies";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                movieList.Add(new MRListData(int.Parse(reader[0].ToString()), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString()));
            }
            command.Dispose();
            return movieList;
        }

        public List<MRListData> getPersonListToMovie(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> personList = new List<MRListData>();
            List<string> personID = new List<string>();
            command.CommandText = "SELECT Person_idPerson, CharacterName, FROM Role WHERE Movies_idMovies = " + idMovies;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                personID.Add(reader[0].ToString());
            }
            return personList;
        }

        public List<MRListData> getPersonList(PersonEnum editable)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> personList = new List<MRListData>();
            command.CommandText = "SELECT idPerson, Name, " + editable.ToString() + " FROM Person";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                personList.Add(new MRListData(int.Parse(reader["idPerson"].ToString()), reader["Name"].ToString(), 0, reader[editable.ToString()].ToString()));
            }
            return personList;
        }

        public GUIPerson getPersonInfo(int idPerson)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM Person WHERE idPerson = " + idPerson;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            int dbID = int.Parse(reader["idPerson"].ToString());
            string Name = reader["Name"].ToString();
            string OriginalName = reader["OriginalName"].ToString();
            string Biography = reader["Biography"].ToString();
            DateTime Birthday; //noch einfügen
            DateTime Deathday;
            bool male;
            int CountryofBirth = int.Parse(reader["CountryofBirth"].ToString());
            string CityofBirth = reader["CityofBirth"].ToString();
            UInt64 lifetimeGross = UInt64.Parse(reader["lifetimeGross"].ToString());
            int boxofficeAverage = int.Parse(reader["boxofficeAverage"].ToString());
            int OscarNominations = int.Parse(reader["OscarNominations"].ToString());
            int OscarWins = int.Parse(reader["OscarWins"].ToString());
            int OtherNominations = int.Parse(reader["OtherNominations"].ToString());
            int OtherWins = int.Parse(reader["OtherWins"].ToString());
            int imdbID = int.Parse(reader["imdbID"].ToString());



            return new GUIPerson(dbID,Name,OriginalName,Biography,CountryofBirth,CityofBirth,lifetimeGross,boxofficeAverage,OscarNominations,OscarWins,OtherNominations,OtherWins,imdbID);
        }

        public bool testAndSetPerson(uint imdbID)
        {
            bool contains;
            lock (this.imdbPersonIds)
            {
                contains = this.imdbPersonIds.Contains(imdbID);
                if (!contains)
                {
                    this.imdbPersonIds.Add(imdbID);
                    SQLiteCommand command = new SQLiteCommand(connection);
                    command.CommandText = "insert into Person (imdbID) values (" + imdbID.ToString() + ")";
                    executeCommandThreadSafe(command);
                }
            }
            return contains;

        }
        public void closeConnection()
        {
            connection.Dispose();
            connection.Close();
        }

        public void insertMovieData(Movie m)
        {
            insertImdbMovie(m.imdbMovie);
            foreach (ImdbPerson person in m.persons)
            {
                updateImdbPerson(person);
            }

            int idMovies = getIdMoviesByImdbId(m.imdbMovie.imdbID);
            if (idMovies > -1)
            {
                foreach (Tuple<uint, string> t in m.imdbMovie.roles.ToArray<Tuple<uint, string>>())
                {
                    insertRole(t.Item1, t.Item2, (uint)idMovies);
                }
            }
            if (m.imdbMovie.poster != null)
            {
                PictureHandler.savePicturePoster(m.imdbMovie.poster, idMovies);
            }

        }

        private void executeCommandThreadSafe(SQLiteCommand command)
        {
            lock (DBProtect)
            {
                command.ExecuteNonQuery();
            }
        }

        private SQLiteDataReader executeReaderThreadSafe(SQLiteCommand command)
        {
            SQLiteDataReader r;
            lock (DBProtect)
            {
                r = command.ExecuteReader();
            }
            return r;
        }


        private int getIdPersonByImdbId(uint personImdbId)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson FROM Person WHERE imdbID=" + personImdbId.ToString();
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            if (reader.Read())
            {
                return int.Parse(reader[0].ToString());
            }
            return -1;
        }

        private int getIdMoviesByImdbId(uint movieImdbId)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idMovies FROM Movies WHERE imdbID = " + movieImdbId.ToString();
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            if (reader.Read())
            {
                return int.Parse(reader[0].ToString());
            }
            return -1;
        }

        private void insertRole(uint personImdbId, String characterName, uint idMovies)
        {
            int idPerson = getIdPersonByImdbId(personImdbId);
            if (idPerson > -1)
            {
                SQLiteCommand command = new SQLiteCommand(connection);

                command.CommandText = "INSERT INTO Role (Person_idPerson, Movies_idMovies, CharacterName, Rank) VALUES(@Person_idPerson, @Movies_idMovies, @CharacterName, @Rank)";
                var param = new SQLiteParameter("@Person_idPerson") { Value = idPerson };
                command.Parameters.Add(param);
                param = new SQLiteParameter("@Movies_idMovies") { Value = idMovies };
                command.Parameters.Add(param);
                param = new SQLiteParameter("@CharacterName") { Value = characterName };
                command.Parameters.Add(param);
                param = new SQLiteParameter("@Rank") { Value = 0 };
                command.Parameters.Add(param);

                executeCommandThreadSafe(command);
            }
        }

        private void updateImdbPerson(ImdbPerson person)
        {

            int idPerson = getIdPersonByImdbId(person.imdbID);
            SQLiteCommand command = new SQLiteCommand(connection);

            command.CommandText = "UPDATE Person SET Name=@Name, " +
                "OriginalName=@OriginalName, " +
                "Biography=@Biography, " +
                "Birthday=@Birthday, " +
                "Deathday=@Deathday, " +
                "male=@male, " +
                "CountryofBirth=@CountryofBirth, " +
                "CityofBirth=@CityofBirth, " +
                "lifetimeGross=@lifetimeGross, " +
                "boxofficeAverage=@boxofficeAverage, " +
                "OscarNominations=@OscarNominations, " +
                "OscarWins=@OscarWins, " +
                "OtherNominations=@OtherNominations, " +
                "OtherWins=@OtherWins, " +
                "PosterPath=@PosterPath" +
                " WHERE idPerson=@idPerson";

            var param = new SQLiteParameter("@Name") { Value = person.name };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@OriginalName") { Value = person.birthname };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@Biography") { Value = "" };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@Birthday") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@Deathday") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@male") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@CountryofBirth") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@CityofBirth") { Value = "" };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@lifetimeGross") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@boxofficeAverage") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@OscarNominations") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@OscarWins") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@OtherNominations") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@OtherWins") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@PosterPath") { Value = "" };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@idPerson") { Value = idPerson };
            command.Parameters.Add(param);

            executeCommandThreadSafe(command);
            if (person.image != null)
            {
                PictureHandler.savePersonPortrait(person.image, idPerson);
            }
        }

        private void insertImdbMovie(ImdbMovie movie)
        {
            SQLiteCommand command = new SQLiteCommand(connection);

            command.CommandText = "INSERT INTO Movies (title, runtime, plot, originalTitle, " +
                "imdbID, imdbRating, imdbRatingVotes, imdbTop250, metacriticsReviewRating, " +
                "metacriticsUsersRating, rottenTomatoesAudience, tomatometer, personalRating, " +
                "year, boxofficeWorldwide, boxofficeAmerica, boxofficeForeign, boxofficeFirstWeekend, " +
                "rangFirstWeekend, rankAllTime, weeksInCinema, otherWins, otherNominations, notes, posterPath, seenCount, " +
                "TVSeries, lastSeen)" +
                "VALUES(@title, @runtime, @plot, @originalTitle, " +
                "@imdbID, @imdbRating, @imdbRatingVotes, @imdbTop250, @metacriticsReviewRating, " +
                "@metacriticsUsersRating, @rottenTomatoesAudience, @tomatometer, @personalRating, " +
                "@year, @boxofficeWorldwide, @boxofficeAmerica, @boxofficeForeign, @boxofficeFirstWeekend, " +
                "@rangFirstWeekend, @rankAllTime, @weeksInCinema, @otherWins, @otherNominations, @notes, @posterPath, @seenCount, " +
                "@TVSeries, @lastSeen)";

            var param = new SQLiteParameter("@title") { Value = movie.title };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@runtime") { Value = movie.runtime };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@plot") { Value = movie.plot };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@originalTitle") { Value = movie.originalTitle };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@imdbID") { Value = movie.imdbID };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@imdbRating") { Value = movie.imdbRating };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@imdbRatingVotes") { Value = movie.imdbRatingVotes };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@imdbTop250") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            /*
             * Meta ID oben entfernt!!
            param = new SQLiteParameter("@metacriticsID") { Value = "" };
            command.Parameters.Add(param);
             * */
            //TODO
            param = new SQLiteParameter("@metacriticsReviewRating") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@metacriticsUsersRating") { Value = 0 };
            command.Parameters.Add(param);

            /*
             * entfernt
            param = new SQLiteParameter("@rottentomatoesID") { Value = 0 };
            command.Parameters.Add(param);
            */
            //TODO
            param = new SQLiteParameter("@rottenTomatoesAudience") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@tomatometer") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@personalRating") { Value = 0 };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@year") { Value = movie.year };
            command.Parameters.Add(param);
            //TODO
            /*
             * oben entfernt
            param = new SQLiteParameter("@boxofficemojoID") { Value = "" };
            command.Parameters.Add(param);
             */
            //TODO
            param = new SQLiteParameter("@boxofficeWorldwide") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@boxofficeAmerica") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@boxofficeForeign") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@boxofficeFirstWeekend") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@rangFirstWeekend") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@rankAllTime") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@weeksInCinema") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@otherWins") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@otherNominations") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@notes") { Value = "" };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@posterPath") { Value = "" };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@seenCount") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@TVSeries") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@lastSeen") { Value = 0 };
            command.Parameters.Add(param);

            executeCommandThreadSafe(command);
        }

        public GUIMovie getMovieInfo(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM Movies WHERE idMovies = " + idMovies;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            int dbId = int.Parse(reader["idMovies"].ToString());
            string title = reader["title"].ToString();
            string originalTitle = reader["originalTitle"].ToString();
            int runtime = int.Parse(reader["runtime"].ToString());
            string plot = reader["plot"].ToString();
            int year = int.Parse(reader["year"].ToString());
            int imdbID = int.Parse(reader["imdbID"].ToString());
            int imdbRating = int.Parse(reader["imdbRating"].ToString());
            int imdbRatingVotes = int.Parse(reader["imdbRatingVotes"].ToString());
            int imdbTop250 = int.Parse(reader["imdbTop250"].ToString());
            string metacriticsID = reader["metacriticsID"].ToString();
            int metacriticsReviewRating = int.Parse(reader["metacriticsReviewRating"].ToString());
            int metacriticsUsersRating = int.Parse(reader["metacriticsUsersRating"].ToString());
            string rottentomatoesID = reader["rottentomatoesID"].ToString();
            int rottenTomatoesAudience = int.Parse(reader["rottenTomatoesAudience"].ToString());
            int tomatometer = int.Parse(reader["tomatometer"].ToString());
            int personalRating = int.Parse(reader["personalRating"].ToString());
            string boxofficemojoID = reader["boxofficemojoID"].ToString();
            UInt32 boxofficeWorldwide = UInt32.Parse(reader["boxofficeWorldwide"].ToString());
            UInt32 boxofficeAmerica = UInt32.Parse(reader["boxofficeAmerica"].ToString());
            UInt32 boxofficeForeign = UInt32.Parse(reader["boxofficeForeign"].ToString());
            int boxofficeFirstWeekend = int.Parse(reader["boxofficeFirstWeekend"].ToString());
            int rangFirstWeekend = int.Parse(reader["rangFirstWeekend"].ToString());
            int rankAllTime = int.Parse(reader["rankAllTime"].ToString());
            int weeksInCinema = int.Parse(reader["weeksInCinema"].ToString());
            int otherWins = int.Parse(reader["otherWins"].ToString());
            int otherNominations = int.Parse(reader["otherNominations"].ToString());
            string notes = reader["notes"].ToString();
            bool TVSeries = true; //noch auslesen
            int seenCount = int.Parse(reader["seenCount"].ToString());
            DateTime lastSeen = new DateTime(); // noch auslesen
            return new GUIMovie(dbId, title, originalTitle, runtime, plot, year, imdbID, imdbRating, imdbRatingVotes, imdbTop250, metacriticsID, metacriticsReviewRating, metacriticsUsersRating, rottentomatoesID, rottenTomatoesAudience, tomatometer, personalRating, boxofficemojoID, boxofficeWorldwide, boxofficeAmerica, boxofficeForeign, boxofficeFirstWeekend, rangFirstWeekend, rankAllTime, weeksInCinema, otherWins, otherNominations, notes, TVSeries, seenCount, lastSeen);
        }

        public void savePictureToHD(Bitmap b, string path, string filename)
        {
            Directory.CreateDirectory(path);
            b.Save(path + "\\" + filename, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

    }
}
