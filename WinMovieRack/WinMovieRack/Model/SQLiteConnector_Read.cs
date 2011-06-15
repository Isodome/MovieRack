using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace WinMovieRack.Model {
	public partial class SQLiteConnector{

		public List<MRListData> getCompleteMovieList(MovieEnum editable, MovieEnum order) {
			SQLiteCommand command = new SQLiteCommand(connection);
			List<MRListData> movieList = new List<MRListData>();
			command.CommandText = "SELECT idMovies, title, year, " + editable.ToString() + " FROM Movies ORDER BY " + order.ToString();
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			while (reader.Read()) {
				movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader[editable.ToString()].ToString()));
			}
			command.Dispose();
			return movieList;
		}

		public List<MRListData> getCompletePersonList(PersonEnum editable, PersonEnum order) {
			SQLiteCommand command = new SQLiteCommand(connection);
			List<MRListData> personList = new List<MRListData>();
			command.CommandText = "SELECT idPerson, Name, Birthday, " + editable.ToString() + " FROM Person ORDER BY " + order.ToString();
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			while (reader.Read()) {
				//Console.WriteLine(reader.GetDateTime(2).Year);//DateTime wird nochnicht gespeichert
				personList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), 10, reader[editable.ToString()].ToString()));
			}
			command.Dispose();
			return personList;
		}

		public List<MRListData> getPersonListToMovie(int idMovies) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "SELECT idPerson, Name, Birthday, CharacterName FROM Role JOIN Person WHERE Person.idPerson = Role.Person_idPerson AND Role.Movies_idMovies =" + idMovies;
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			List<MRListData> movieList = new List<MRListData>();
			while (reader.Read()) {
				//Console.WriteLine(reader.GetDateTime(2).Year);//DateTime wird nochnicht gespeichert
				movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), 10, reader.GetString(3)));
			}
			command.Dispose();
			return movieList;
		}



		public List<MRListData> getMovieListToPerson(int Person_idPerson) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "SELECT idMovies, title, year, CharacterName FROM Role JOIN Movies WHERE Movies.idMovies = Role.Movies_idMovies AND Role.Person_idPerson =" + Person_idPerson;
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			List<MRListData> movieList = new List<MRListData>();
			while (reader.Read()) {
				movieList.Add(new MRListData(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3)));
			}
			command.Dispose();
			return movieList;
		}

		public List<String> getGenresToMovie(int idMovies) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "SELECT Genre FROM Genre JOIN Movies_has_Genre WHERE Movies_has_Genre.Movies_idMovies = " + idMovies + " AND Movies_has_Genre.Genre_idGenre = Genre.idGenre";
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			List<String> genreList = new List<String>();
			Console.WriteLine(idMovies);
			while (reader.Read()) {
				genreList.Add(reader.GetString(0));
			}
			command.Dispose();
			return genreList;
		}

		public List<String> getAlsoKnownToMovie(int idMovies) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "SELECT AlsoKnownAs FROM AlsoKownAs JOIN Movies_has_AlsoKownAs WHERE Movies_has_AlsoKownAs.Movies_idMovies = " + idMovies + " AND Movies_has_AlsoKownAs.AlsoKownAs_idAlsoKownAs = AlsoKownAs.idAlsoKownAs";
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			reader = executeReaderThreadSafe(command);
			List<String> alsoKnownList = new List<String>();
			while (reader.Read()) {
				alsoKnownList.Add(reader.GetString(0));
			}
			command.Dispose();
			return alsoKnownList;
		}

		public List<String> getLanguageToMovie(int idMovies) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "SELECT Language FROM Language JOIN Movies_has_Language WHERE Movies_has_Language.Movies_idMovies = " + idMovies + " AND Movies_has_Language.Language_idLanguage = Language.idLanguage";
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			reader = executeReaderThreadSafe(command);
			List<String> languageList = new List<String>();
			while (reader.Read()) {
				languageList.Add(reader.GetString(0));
			}
			command.Dispose();
			return languageList;
		}

		public DataSet getAwardstoPerson(int idPerson) {
			SQLiteCommand command = new SQLiteCommand("SELECT title, originalTitle, year,imdbRating FROM Movies", connection);
			DataSet personAwards = new DataSet();
			SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(command);
			sqlDataAdapter.Fill(personAwards, "personAwards");
			return personAwards;
		}

		public DataSet getAwardstoMovie(int idMovies) {
			SQLiteCommand command = new SQLiteCommand("SELECT Name, Year, isWin, category, award FROM OtherAwards JOIN Person_has_OtherAwards JOIN Person WHERE OtherAwards.Movies_idMovies = " + idMovies + " AND  OtherAwards.idOtherAwards = Person_has_OtherAwards.OtherAwards_idOtherAwards AND Person_has_OtherAwards.Person_idPerson = Person.idPerson", connection);
			DataSet movieAwards = new DataSet();
			SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(command);
			sqlDataAdapter.Fill(movieAwards, "movieAwards");
			return movieAwards;
		}

		public GUIPerson getPersonInfo(int idPerson) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "SELECT * FROM Person WHERE idPerson = " + idPerson;
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			int dbID = int.Parse(reader["idPerson"].ToString());
			string Name = reader["Name"].ToString();
			string OriginalName = reader["OriginalName"].ToString();
			string Biography = reader["Biography"].ToString();
			DateTime Birthday; //noch einfügen
			DateTime Deathday;
			char gender = reader[PersonEnum.gender.ToString()].ToString()[0];
			int CountryofBirth = int.Parse(reader["CountryofBirth"].ToString());
			string CityofBirth = reader["CityofBirth"].ToString();
			UInt64 lifetimeGross = UInt64.Parse(reader["lifetimeGross"].ToString());
			int boxofficeAverage = int.Parse(reader["boxofficeAverage"].ToString());
			int OscarNominations = int.Parse(reader["OscarNominations"].ToString());
			int OscarWins = int.Parse(reader["OscarWins"].ToString());
			int OtherNominations = int.Parse(reader["OtherNominations"].ToString());
			int OtherWins = int.Parse(reader["OtherWins"].ToString());
			int imdbID = int.Parse(reader["imdbID"].ToString());
			command.Dispose();
			return new GUIPerson(dbID, Name, OriginalName, Biography, CountryofBirth, CityofBirth, lifetimeGross, boxofficeAverage, OscarNominations, OscarWins, OtherNominations, OtherWins, imdbID);
		}

		public GUIMovie getMovieInfo(int idMovies) {
			SQLiteCommand command = new SQLiteCommand(connection);
			//command.CommandText = "SELECT * FROM Movies JOIN MovieAgeRating WHERE Movies.idMovies = " + idMovies + " AND Movies.idMovies = MovieAgeRating.Movies_idMovies";
			command.CommandText = "SELECT * FROM Movies WHERE idMovies = " + idMovies; //Anderen Befehl nutzen, wenn Altersfreigaben eingefügt werden.
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
			// String movieAgeRating = reader["MovieAgeRating"].ToString(); //Einkommentieren wenn Altersfreigaben eingefügt werden.

			return new GUIMovie(dbId, title, originalTitle, runtime, plot, year, imdbID, imdbRating, imdbRatingVotes, imdbTop250, metacriticsID, metacriticsReviewRating, metacriticsUsersRating, rottentomatoesID, rottenTomatoesAudience, tomatometer, personalRating, boxofficemojoID, boxofficeWorldwide, boxofficeAmerica, boxofficeForeign, boxofficeFirstWeekend, rangFirstWeekend, rankAllTime, weeksInCinema, otherWins, otherNominations, notes, TVSeries, seenCount, lastSeen);
		}

	}
}
