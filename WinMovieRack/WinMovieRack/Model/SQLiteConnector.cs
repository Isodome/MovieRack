using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
namespace WinMovieRack.Model {
    public class SQLiteConnector {
        private SQLiteConnection connection;
        private HashSet<uint> imdbPersonIds;

        public void initDb() {
            string dataSource = "MovieRack.db";
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + dataSource;
            connection.Open();
            this.checkTables();

            imdbPersonIds = new HashSet<uint>();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "select imdbID FROM Person";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                this.imdbPersonIds.Add(uint.Parse(reader[0].ToString()));
            }

        }

        public void checkTables() {
            FileInfo file = new FileInfo("createscript.sql");
            string script = file.OpenText().ReadToEnd();

            SQLiteCommand command = new SQLiteCommand(connection);


            // Erstellen der Tabelle, sofern diese noch nicht existiert.
            command.CommandText = script;
            command.ExecuteNonQuery();

            // Einfügen eines Test-Datensatzes.
            command.CommandText = "INSERT INTO Movies (idMovies, year, title, plot, runtime, posterPath) VALUES(NULL, 1970, 'James Bond xx', 'laaangweilig', 120, 'Z:\\MovieRack\\WinMovieRack\\WinMovieRack\\Images\\#9.jpg')";
            command.ExecuteNonQuery();

            command.CommandText = "SELECT Plot, Title FROM Movies WHERE Year = 1970";

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                Console.WriteLine("Dies ist der {0}. eingefügte Datensatz mit dem Wert: \"{1}\"", reader[0].ToString(), reader[1].ToString());
            }
            // Freigabe der Ressourcen.
            command.Dispose();

        }

        public List<MRListData> getMovieList(MovieEnum editable) {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> movieList = new List<MRListData>();
            command.CommandText = "SELECT idMovies, title, year, " + editable.ToString() + ", posterPath FROM Movies";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                movieList.Add(new MRListData(int.Parse(reader[0].ToString()), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString(), reader[4].ToString()));
            }
            command.Dispose();
            return movieList;
        }

        public List<MRListData> getPersonList(int idMovies) {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> personList = new List<MRListData>();
            List<string> personID = new List<string>();
            command.CommandText = "SELECT Person_idPerson, CharacterName, FROM Role WHERE Movies_idMovies = " + idMovies;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                personID.Add(reader[0].ToString());
            }
            return personList;
        }

        public bool testAndSetPerson(uint imdbID) {
            bool contains;
            lock (this.imdbPersonIds) {
                contains = this.imdbPersonIds.Contains(imdbID);
                if (!contains) {
                    this.imdbPersonIds.Add(imdbID);
                    SQLiteCommand command = new SQLiteCommand(connection);
                    command.CommandText = "insert into Person (imdbID) values (" + imdbID.ToString() + ")";
                    command.ExecuteNonQuery();
                }
            }
            return contains;

        }
        public void closeConnection() {
            connection.Dispose();
            connection.Close();
        }
        public void insertMovieData(Movie m) {
            insertImdbMovie(m.imdbMovie);
            foreach (ImdbPerson person in m.persons) {
                updateImdbPerson(person);
            }

            int idMovies = getIdMoviesByImdbId(m.imdbMovie.imdbID);
            if (idMovies > -1) {
                foreach (Tuple<uint, string> t in m.imdbMovie.roles.ToArray<Tuple<uint, string>>()) {
                    insertRole(t.Item1, t.Item2, (uint)idMovies);
                }
            }

        }

        private int getIdPersonByImdbId(uint personImdbId) {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson FROM Person WHERE imdbID = " + personImdbId.ToString();
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                return int.Parse(reader[0].ToString());
            }
            return -1;
        }

        private int getIdMoviesByImdbId(uint movieImdbId) {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idMovies FROM Movies WHERE imdbID = " + movieImdbId.ToString();
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                return int.Parse(reader[0].ToString());
            }
            return -1;
        }

        private void insertRole(uint personImdbId, String characterName, uint idMovies) {
            int idPerson = getIdPersonByImdbId(personImdbId);
            if (idPerson > -1) {
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

                command.ExecuteNonQuery();
            }
        }

        private void updateImdbPerson(ImdbPerson person) {
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
                " WHERE imdbID=@imdbID";

            var param = new SQLiteParameter("@Name") { Value = person.name };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@OriginalName") { Value = person.birthname };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@Biography") { Value = "" };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@Birthday") { Value = person.birthday };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@Deathday") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@male") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@CountryofBirth") { Value = "" };
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
            param = new SQLiteParameter("@imdbID") { Value = person.imdbID };
            command.Parameters.Add(param);

            command.ExecuteNonQuery();

        }

        private void insertImdbMovie(ImdbMovie movie) {
            SQLiteCommand command = new SQLiteCommand(connection);

            command.CommandText = "INSERT INTO Movies (title, runtime, plot, originalTitle, " +
                "imdbID, imdbRating, imdbRatingVotes, imdbTop250, metacriticsID, metacriticsReviewRating, " +
                "metacriticsUsersRating, rottentomatoesID, rottenTomatoesAudience, tomatometer, personalRating, " +
                "year, boxofficemojoID, boxofficeWorldwide, boxofficeAmerica, boxofficeForeign, boxofficeFirstWeekend, " +
                "rangFirstWeekend, rankAllTime, weeksInCinema, otherWins, otherNominations, notes, posterPath, seenCount, " +
                "TVSeries, lastSeen)" +
                "VALUES(@title, @runtime, @plot, @originalTitle, " +
                "@imdbID, @imdbRating, @imdbRatingVotes, @imdbTop250, @metacriticsID, @metacriticsReviewRating, " +
                "@metacriticsUsersRating, @rottentomatoesID, @rottenTomatoesAudience, @tomatometer, @personalRating, " +
                "@year, @boxofficemojoID, @boxofficeWorldwide, @boxofficeAmerica, @boxofficeForeign, @boxofficeFirstWeekend, " +
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
            param = new SQLiteParameter("@metacriticsID") { Value = "" };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@metacriticsReviewRating") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@metacriticsUsersRating") { Value = 0 };
            command.Parameters.Add(param);
            //TODO
            param = new SQLiteParameter("@rottentomatoesID") { Value = 0 };
            command.Parameters.Add(param);
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
            param = new SQLiteParameter("@boxofficemojoID") { Value = "" };
            command.Parameters.Add(param);
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

            command.ExecuteNonQuery();
        }

    }
}
