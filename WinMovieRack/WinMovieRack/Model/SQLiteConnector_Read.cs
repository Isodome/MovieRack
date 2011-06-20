using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace WinMovieRack.Model
{
	
    public partial class SQLiteConnector
    {
		public delegate void MRToDo(MRListData t);
        public List<MRListData> getCompleteMovieList(MovieEnum editable, MovieEnum order)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> movieList = new List<MRListData>();
            command.CommandText = "SELECT idMovies, title, year, " + editable.ToString() + " FROM Movies ORDER BY " + order.ToString();
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {

                int idMovies = reader.GetInt32(0);
                string title = reader.GetString(1);
                int year = reader.GetInt32(2);
                string edit = reader[editable.ToString()].ToString();
                movieList.Add(new MRListData(idMovies, title, year, edit));
            }
            command.Dispose();
            return movieList;
        }

		public void completeMovieListForEach() {

		}

        public List<MRListData> getCompletePersonList(PersonEnum editable, PersonEnum order)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> personList = new List<MRListData>();
            command.CommandText = "SELECT idPerson, Name, Birthday, " + editable.ToString() + " FROM Person ORDER BY " + order.ToString();
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                int years = 0;
                if (reader.GetDateTime(2).Year != 1)
                {
                    DateTime now = DateTime.Today;
                    years = now.Year - reader.GetDateTime(2).Year;
                    if (reader.GetDateTime(2) > now.AddYears(-years)) years--;
                }
                personList.Add(new MRListData(reader.GetInt32(0), reader["Name"].ToString(), years, reader[editable.ToString()].ToString()));
            }
            command.Dispose();
            return personList;
        }

        public List<MRListData> getPersonListToMovie(int idMovies, int year)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson, Name, Birthday, CharacterName FROM Role JOIN Person WHERE Person.idPerson = Role.Person_idPerson AND Role.Movies_idMovies =" + idMovies;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<MRListData> movieList = new List<MRListData>();
            while (reader.Read())
            {
                int idPerson = reader.GetInt32(0);
                string name = reader.GetString(1); ;
                int years = 0;
                if (reader.GetDateTime(2).Year != 1)
                {
                    years = year - reader.GetDateTime(2).Year;
                }
                string edit = reader.GetString(3);
                movieList.Add(new MRListData(idPerson, name, years, edit));
            }
            command.Dispose();
            return movieList;
        }



        public List<MRListData> getMovieListToPerson(int Person_idPerson)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idMovies, title, year, CharacterName FROM Role JOIN Movies WHERE Movies.idMovies = Role.Movies_idMovies AND Role.Person_idPerson =" + Person_idPerson;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<MRListData> movieList = new List<MRListData>();
            while (reader.Read())
            {
                movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3)));
            }
            command.Dispose();
            return movieList;
        }

        public List<String> getGenresToMovie(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT Genre FROM Genre JOIN Movies_has_Genre WHERE Movies_has_Genre.Movies_idMovies = " + idMovies + " AND Movies_has_Genre.Genre_idGenre = Genre.idGenre";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<String> genreList = new List<String>();
            while (reader.Read())
            {
                genreList.Add(reader.GetString(0));
            }
            command.Dispose();
            return genreList;
        }

        public List<String> getAlsoKnownToMovie(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT AlsoKnownAs FROM AlsoKownAs JOIN Movies_has_AlsoKownAs WHERE Movies_has_AlsoKownAs.Movies_idMovies = " + idMovies + " AND Movies_has_AlsoKownAs.AlsoKownAs_idAlsoKownAs = AlsoKownAs.idAlsoKownAs";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<String> alsoKnownList = new List<String>();
            while (reader.Read())
            {
                alsoKnownList.Add(reader.GetString(0));
            }
            command.Dispose();
            return alsoKnownList;
        }

        public List<String> getLanguageToMovie(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT Language FROM Language JOIN Movies_has_Language WHERE Movies_has_Language.Movies_idMovies = " + idMovies + " AND Movies_has_Language.Language_idLanguage = Language.idLanguage";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<String> languageList = new List<String>();
            while (reader.Read())
            {
                languageList.Add(reader.GetString(0));
            }
            command.Dispose();
            return languageList;
        }

        public DataSet getOtherAwardstoMovie(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT Name, Year, isWin, category, award FROM OtherAwards JOIN Person_has_OtherAwards JOIN Person WHERE OtherAwards.Movies_idMovies = " + idMovies + " AND  OtherAwards.idOtherAwards = Person_has_OtherAwards.OtherAwards_idOtherAwards AND Person_has_OtherAwards.Person_idPerson = Person.idPerson", connection);
            DataSet movieAwards = new DataSet();
            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(command);
            sqlDataAdapter.Fill(movieAwards, "movieAwards");
            command.Dispose();
            return movieAwards;
        }

        public DataSet getOscarstoMovie(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT Name, Year, isWin, category FROM Oscars JOIN Person_has_Oscars JOIN Person WHERE Oscars.Movies_idMovies = " + idMovies + " AND  Oscars.idOscar = Person_has_Oscars.Oscars_idOscar AND Person_has_Oscars.Person_idPerson = Person.idPerson", connection);
            DataSet oscarAwards = new DataSet();
            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(command);
            sqlDataAdapter.Fill(oscarAwards, "movieAwards");
            command.Dispose();
            return oscarAwards;
        }

        public List<MRListData> getStarsListToMovie(int idMovies, int year)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson, Name, Birthday, CharacterName FROM Stars JOIN Person JOIN Role WHERE Person.idPerson = Stars.Person_idPerson AND Person.idPerson = Role.Person_idPerson AND Stars.Movies_idMovies =" + idMovies + " AND Role.Movies_idMovies =" + idMovies;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<MRListData> movieList = new List<MRListData>();
            while (reader.Read())
            {
                int years = 0;
                if (reader.GetDateTime(2).Year != 1)
                {
                    years = year - reader.GetDateTime(2).Year;
                }
                movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), years, reader.GetString(3)));
            }
            command.Dispose();
            return movieList;
        }

        public List<MRListData> getStarsListToMovie1(int idMovies, int year)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT DISTINCT idPerson, Name, Birthday FROM Person JOIN Director JOIN Writer WHERE Writer.Person_idPerson = Director.Person_idPerson AND Person.idPerson = Writer.Person_idPerson AND Director.Movies_idMovies =" + idMovies;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<MRListData> movieList = new List<MRListData>();
            while (reader.Read())
            {
                int years = 0;
                if (reader.GetDateTime(2).Year != 1)
                {
                    years = year - reader.GetDateTime(2).Year;
                }
                movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), years, "Writer, Director"));
            }
            command.Dispose();
            return movieList;
        }


        public List<MRListData> getProductionListToMovie(int idMovies, int year)
        {
            List<MRListData> test = new List<MRListData>();
            test = getStarsListToMovie1(idMovies, year);
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson, Name, Birthday FROM Director JOIN Person WHERE Person.idPerson = Director.Person_idPerson AND Director.Movies_idMovies =" + idMovies;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            List<MRListData> movieList = new List<MRListData>();
            movieList = test;
            while (reader.Read())
            {
                int years = 0;
                if (reader.GetDateTime(2).Year != 1)
                {
                    years = year - reader.GetDateTime(2).Year;
                }
                int count = test.Count;
                for (int i = 0; i < test.Count; i++)
                {
                    if (test.ElementAt(i).dbItemID != reader.GetInt32(0))
                    {
                        count--;
                    }
                }
                if (count == 0)
                {
                    movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), years, "Director"));
                }
            }
            command.Dispose();
            command.CommandText = "SELECT idPerson, Name, Birthday FROM Writer JOIN Person WHERE Person.idPerson = Writer.Person_idPerson AND Writer.Movies_idMovies =" + idMovies;
            reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                int years = 0;
                if (reader.GetDateTime(2).Year != 1)
                {
                    DateTime now = DateTime.Today;
                    years = now.Year - reader.GetDateTime(2).Year;
                    if (reader.GetDateTime(2) > now.AddYears(-years)) years--;
                }
                int count = test.Count;
                for (int i = 0; i < test.Count; i++)
                {
                    if (test.ElementAt(i).dbItemID != reader.GetInt32(0))
                    {
                        count--;
                    }
                }
                if (count == 0)
                {
                    movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), years, "Writer"));
                }
            }
            return movieList;
        }


        public GUIPerson getPersonInfo(int idPerson)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson, Name, OriginalName, Biography, Birthday, Deathday, gender, CountryofBirth, CityofBirth, lifetimeGross, boxofficeAverage, OscarNominations, OscarWins, OtherNominations, OtherWins, imdbID FROM Person WHERE idPerson = " + idPerson;
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            int dbID = 0;
            string Name = "";
            string OriginalName = "";
            string Biography = ""; ;
            char gender = 'n';
            int CountryofBirth = 0;
            string CityofBirth = ""; ;
            Int64 lifetimeGross = new Int64();
            int boxofficeAverage = 0;
            int OscarNominations = 0;
            int OscarWins = 0;
            int OtherNominations = 0;
            int OtherWins = 0;
            int imdbID = 0;
            DateTime birthday = DateTime.Today;
            DateTime deathday = DateTime.Today;
            while (reader.Read())
            {
                int i = 0;
                dbID = reader.GetInt32(i++);
                Name = reader.GetString(i++);
                OriginalName = reader.GetString(i++);
                Biography = reader.GetString(i++);
                birthday = reader.GetDateTime(i++);
                deathday = reader.GetDateTime(i++);
                gender = reader.GetChar(i++);
                CountryofBirth = reader.GetInt32(i++);
                CityofBirth = reader.GetString(i++);
                lifetimeGross = reader.GetInt64(i++);
                boxofficeAverage = reader.GetInt32(i++);
                OscarNominations = reader.GetInt32(i++);
                OscarWins = reader.GetInt32(i++);
                OtherNominations = reader.GetInt32(i++);
                OtherWins = reader.GetInt32(i++);
                imdbID = reader.GetInt32(i++);

            }
            command.Dispose();

            return new GUIPerson(dbID, Name, OriginalName, Biography, birthday, deathday, gender, CountryofBirth, CityofBirth, lifetimeGross, boxofficeAverage, OscarNominations, OscarWins, OtherNominations, OtherWins, imdbID);
        }

        public GUIMovie getMovieInfo(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            //command.CommandText = "SELECT * FROM Movies JOIN MovieAgeRating WHERE Movies.idMovies = " + idMovies + " AND Movies.idMovies = MovieAgeRating.Movies_idMovies";
            command.CommandText = "SELECT * FROM Movies WHERE idMovies = " + idMovies; //Anderen Befehl nutzen, wenn Altersfreigaben eingefügt werden.
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            int dbId = int.Parse(reader["idMovies"].ToString());
            string title = reader["title"].ToString();
            string originalTitle = reader["originalTitle"].ToString();
            int runtime = int.Parse(reader["runtime"].ToString());
            string plot = reader["plot"].ToString();

            int year;
            if (!int.TryParse(reader["year"].ToString(), out year))
            {
                year = Symbols.NO_YEAR;
            }

            int imdbID;
            if (!int.TryParse(reader["imdbID"].ToString(), out imdbID))
            {
                imdbID = 0;
            }

            int imdbRating;
            if (!int.TryParse(reader["imdbRating"].ToString(), out imdbRating))
            {
                imdbRating = Symbols.NO_IMDB_RATING;
            }

            int imdbRatingVotes;
            if (!int.TryParse(reader["imdbRatingVotes"].ToString(), out imdbRatingVotes))
            {
                imdbRatingVotes = Symbols.NO_IMDB_RATING;
            }

            int imdbTop250;
            if (!int.TryParse(reader["imdbTop250"].ToString(), out imdbTop250))
            {
                imdbTop250 = Symbols.NO_TOP250;
            }

            string metacriticsID = reader["metacriticsID"].ToString();
            int metacriticsReviewRating = int.Parse(reader["metacriticsReviewRating"].ToString());
            int metacriticsUsersRating = int.Parse(reader["metacriticsUsersRating"].ToString());
            int metacriticsReviewVotes = int.Parse(reader["metacriticsReviewVotes"].ToString());
            int metacriticsUserVotes = int.Parse(reader["metacriticsUserVotes"].ToString());

            string rottentomatoesID = reader["rottentomatoesID"].ToString();
            int rottenTomatoesAudience = int.Parse(reader["rottenTomatoesAudience"].ToString());
            int tomatometer = int.Parse(reader["tomatometer"].ToString());
            int rottenTomatoesAudienceVotes = int.Parse(reader["rottenTomatoesAudienceVotes"].ToString());
            int tomatometerVotes = int.Parse(reader["tomatometerVotes"].ToString());


            int personalRating;
            if (!int.TryParse(reader["personalRating"].ToString(), out personalRating))
            {
                personalRating = Symbols.NO_PERSONAL_RATING;
            }

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
            bool TVSeries = true;
            int seenCount = int.Parse(reader["seenCount"].ToString());
            DateTime lastSeen;
            if (!DateTime.TryParse(reader["lastSeen"].ToString(), out lastSeen))
            {
                lastSeen = new DateTime();
            }

            UInt32 budget = UInt32.Parse(reader["budget"].ToString());
            // String movieAgeRating = reader["MovieAgeRating"].ToString(); //Einkommentieren wenn Altersfreigaben eingefügt werden.

            return new GUIMovie(dbId, title, originalTitle, runtime, plot, year, imdbID, imdbRating, imdbRatingVotes, imdbTop250, metacriticsID, metacriticsReviewRating, metacriticsUsersRating, metacriticsReviewVotes, metacriticsUserVotes, rottentomatoesID, rottenTomatoesAudience, tomatometer, rottenTomatoesAudienceVotes, tomatometerVotes, personalRating, boxofficemojoID, boxofficeWorldwide, boxofficeAmerica, boxofficeForeign, boxofficeFirstWeekend, rangFirstWeekend, rankAllTime, weeksInCinema, otherWins, otherNominations, notes, TVSeries, seenCount, lastSeen, budget);
        }

    }
}
