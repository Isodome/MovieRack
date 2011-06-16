using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using WinMovieRack.Model.Enums;
namespace WinMovieRack.Model {
	public partial class SQLiteConnector {

		private const string insertInOscars = "INSERT INTO Oscars (Year, isWin, category, Movies_idMovies) VALUES(@Year, @isWin, @category, @Movies_idMovies)";
		private const string insertInOtherWins = "INSERT INTO OtherAwards (Year, isWin, category, Movies_idMovies, award) VALUES(@Year, @isWin, @category, @Movies_idMovies, @award)";


		public void updateMovieData(Movie m) {
			beginTransaction();
			int idMovies = getIdMoviesByImdbId(m.imdbMovie.imdbID);

			updateImdbMovie(m.imdbMovie, idMovies);
			foreach (ImdbPerson person in m.persons) {
				updateImdbPerson(person);
			}

		

			if (idMovies > -1) {
				foreach (Tuple<uint, string> t in m.imdbMovie.roles) {
					insertRole(t.Item1, t.Item2, idMovies);
				}
				foreach (uint imdb in m.imdbMovie.directors) {
					int idPerson = getIdPersonByImdbId(imdb);
					insertMoviePersonRelation(PersonMovieRelations.Director, idPerson, idMovies);
				}
				foreach (uint id in m.imdbMovie.writers) {
					int idPerson = getIdPersonByImdbId(id);
					insertMoviePersonRelation(PersonMovieRelations.Writer, idPerson, idMovies);
				}
				foreach (uint id in m.imdbMovie.stars) {
					int idPerson = getIdPersonByImdbId(id);
					insertMoviePersonRelation(PersonMovieRelations.Stars, idPerson, idMovies);
				}
				updateCountriesToMovie(idMovies, m.imdbMovie.countries);
				updateGenresToMovie(idMovies, m.imdbMovie.genres);
				updateLanguageToMovie(idMovies, m.imdbMovie.languages);
				updateAwardsToMovie(idMovies, m.imdbMovie.awards);
			}
			if (m.imdbMovie.poster != null) {
				PictureHandler.saveMoviePoster(m.imdbMovie.poster, idMovies);
			}
			endTransaction();
			Console.WriteLine("Done inserting {0} into DB", m.imdbMovie.title);

		}

		private void updateAwardsToMovie(int idMovies, List<Award> awards) {

			foreach (Award a in awards) {
				SQLiteCommand command = new SQLiteCommand(connection);
				var param = new SQLiteParameter();
				if (a.isOscar) {
					command.CommandText = insertInOscars;
				} else {
					command.CommandText = insertInOtherWins;
					param = new SQLiteParameter("@award") { Value = a.award };
					command.Parameters.Add(param);
				}

				param = new SQLiteParameter("@Year") { Value = a.year };
				command.Parameters.Add(param);
				param = new SQLiteParameter("@category") { Value = a.category };
				command.Parameters.Add(param);
				param = new SQLiteParameter("@isWin") { Value = a.won };
				command.Parameters.Add(param);
				param = new SQLiteParameter("@Movies_idMovies") { Value = idMovies };
				command.Parameters.Add(param);
				
				int idAward = executeCommandAndReturnID(command);
				

				foreach (uint imdbID in a.persons) {
					int idPerson = getIdPersonByImdbId(imdbID);
					SQLiteCommand persCmnd = new SQLiteCommand(connection);
					
					if (a.isOscar) {
						persCmnd.CommandText = "INSERT INTO Person_has_Oscars (Person_idPerson, Oscars_idOscar) VALUES(@Person_idPerson, @Oscars_idOscar);";
						param = new SQLiteParameter("@Oscars_idOscar") { Value = idAward };
						persCmnd.Parameters.Add(param);
					} else {
						persCmnd.CommandText = "INSERT INTO Person_has_OtherAwards (Person_idPerson, OtherAwards_idOtherAwards) VALUES (@Person_idPerson, @OtherAwards_idOtherAwards);";
						param = new SQLiteParameter("@OtherAwards_idOtherAwards") { Value = idAward };
						persCmnd.Parameters.Add(param);
					}
					param = new SQLiteParameter("@Person_idPerson") { Value = idPerson};
					persCmnd.Parameters.Add(param);
					executeCommandThreadSafe(persCmnd);
				}

			}
		}

		private void updateCountriesToMovie(int idMovies, List<string> countries) {
			foreach (string country in countries) {
				int idCountry = getIDCountryByCountryName(country);
				if (idCountry == -1) {
					SQLiteCommand cmdInsert = new SQLiteCommand(connection);
					cmdInsert.CommandText = String.Format("INSERT INTO Country (Country) VALUES('{0}')", country);
					executeCommandThreadSafe(cmdInsert);
					idCountry = getIDCountryByCountryName(country);
				}

				SQLiteCommand command = new SQLiteCommand(connection);
				command.CommandText = string.Format("INSERT OR IGNORE INTO Country_has_Movies (Country_idCountry, Movies_idMovies) VALUES(@Country_idCountry, @Movies_idMovies)");
				var param = new SQLiteParameter("@Country_idCountry") { Value = idCountry };
				command.Parameters.Add(param);
				param = new SQLiteParameter("@Movies_idMovies") { Value = idMovies };
				command.Parameters.Add(param);
				executeCommandThreadSafe(command);

			}
		}

		private void updateGenresToMovie(int idMovies, List<string> genres) {
			foreach (string genre in genres) {
				int idGenre = getIDGenreByGenre(genre);
				if (idGenre == -1) {
					SQLiteCommand cmdInsert = new SQLiteCommand(connection);
					cmdInsert.CommandText = String.Format("INSERT INTO Genre (Genre) VALUES('{0}')", genre);
					executeCommandThreadSafe(cmdInsert);
					idGenre = getIDGenreByGenre(genre);
				}

				SQLiteCommand command = new SQLiteCommand(connection);
				command.CommandText = string.Format("INSERT OR IGNORE INTO Movies_has_Genre (Genre_idGenre, Movies_idMovies) VALUES(@Genre_idGenre, @Movies_idMovies)");
				var param = new SQLiteParameter("@Genre_idGenre") { Value = idGenre };
				command.Parameters.Add(param);
				param = new SQLiteParameter("@Movies_idMovies") { Value = idMovies };
				command.Parameters.Add(param);
				executeCommandThreadSafe(command);

			}
		}

		private void updateLanguageToMovie(int idMovies, List<string> languages) {
			foreach (string lang in languages) {
				int idLanguage = getIDLanguageByLanguage(lang);
				if (idLanguage == -1) {
					SQLiteCommand cmdInsert = new SQLiteCommand(connection);
					cmdInsert.CommandText = String.Format("INSERT OR IGNORE INTO Language (Language) VALUES('{0}')", lang);
					executeCommandThreadSafe(cmdInsert);
					idLanguage = getIDLanguageByLanguage(lang);
				}

				SQLiteCommand command = new SQLiteCommand(connection);
				command.CommandText = string.Format("INSERT OR IGNORE INTO Movies_has_Language (Language_idLanguage, Movies_idMovies) VALUES(@Language_idLanguage, @Movies_idMovies)");
				var param = new SQLiteParameter("@Language_idLanguage") { Value = idLanguage };
				command.Parameters.Add(param);
				param = new SQLiteParameter("@Movies_idMovies") { Value = idMovies };
				command.Parameters.Add(param);
				executeCommandThreadSafe(command);

			}
		}



		public void updateImdbPerson(ImdbPerson person) {

			SQLiteCommand command = new SQLiteCommand(connection);

			command.CommandText = "UPDATE Person SET Name=@Name, " +
				"OriginalName=@OriginalName, " +
				"Biography=@Biography, " +
				"Birthday=@Birthday, " +
				"Deathday=@Deathday, " +
				"gender=@gender, " +
				"CountryofBirth=@CountryofBirth, " +
				"CityofBirth=@CityofBirth, " +
				"lifetimeGross=@lifetimeGross, " +
				"boxofficeAverage=@boxofficeAverage, " +
				"OscarNominations=@OscarNominations, " +
				"OscarWins=@OscarWins, " +
				"OtherNominations=@OtherNominations, " +
				"OtherWins=@OtherWins " +
				" WHERE idPerson=@idPerson";

			var param = new SQLiteParameter("@Name") { Value = person.name };
			command.Parameters.Add(param);
			param = new SQLiteParameter("@OriginalName") { Value = person.birthname };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@Biography") { Value = "" };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@Birthday") { Value = person.birthday };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@Deathday") { Value = person.deathday };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@gender") { Value = person.gender };
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
			param = new SQLiteParameter("@idPerson") { Value = person.idPerson };
			command.Parameters.Add(param);

			executeCommandThreadSafe(command);
			if (person.image != null) {
				PictureHandler.savePersonPortrait(person.image, person.idPerson);
			}
		}

		private void insertRole(uint personImdbId, String characterName, int idMovies) {
			int idPerson = getIdPersonByImdbId(personImdbId);
			if (idPerson > -1) {
				SQLiteCommand command = new SQLiteCommand(connection);

				command.CommandText = "INSERT OR IGNORE INTO Role (Person_idPerson, Movies_idMovies, CharacterName, Rank) VALUES(@Person_idPerson, @Movies_idMovies, @CharacterName, @Rank)";
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

		private void insertMoviePersonRelation(PersonMovieRelations table , int idPerson, int idMovies) {
		
			SQLiteCommand command = new SQLiteCommand(connection);

			command.CommandText = String.Format("INSERT OR IGNORE INTO {0} (Person_idPerson, Movies_idMovies) VALUES(@Person_idPerson, @Movies_idMovies)", table.ToString());
			var param = new SQLiteParameter("@Person_idPerson") { Value = idPerson };
			command.Parameters.Add(param);
			param = new SQLiteParameter("@Movies_idMovies") { Value = idMovies };
			command.Parameters.Add(param);

			executeCommandThreadSafe(command);

		}


		private void updateImdbMovie(ImdbMovie movie, int idMovies) {
			SQLiteCommand command = new SQLiteCommand(connection);

			command.CommandText = "UPDATE Movies SET " +
				"title=@title, " + 
				"runtime=@runtime, " +
				"plot=@plot, " +
				"originalTitle=@originalTitle, " +
				"imdbRating=@imdbRating, " +
				"imdbRatingVotes=@imdbRatingVotes, " +
				"imdbTop250=@imdbTop250, " +
				"metacriticsReviewRating=@metacriticsReviewRating, " +
				"metacriticsUsersRating=@metacriticsUsersRating, " +
				"rottenTomatoesAudience=@rottenTomatoesAudience, " +
				"year=@year, " +
				"tomatometer=@tomatometer, " +
				"boxofficeWorldwide=@boxofficeWorldwide, " +
				"personalRating=@personalRating, " +
				"boxofficeAmerica=@boxofficeAmerica, " +
				"boxofficeFirstWeekend=@boxofficeFirstWeekend, " +
				"boxofficeForeign=@boxofficeForeign, " +
				"rangFirstWeekend=@rangFirstWeekend, " +
				"rankAllTime=@rankAllTime, " +
				"weeksInCinema=@weeksInCinema, " +
				"otherWins=@otherWins, " +
				"otherNominations=@otherNominations, " +
				"notes=@notes, " +
				"seenCount=@seenCount, " +
				"TVSeries=@TVSeries, " +
				"lastSeen=@lastSeen " +
				"WHERE idMovies=@idMovies";


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
			param = new SQLiteParameter("@seenCount") { Value = 0 };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@TVSeries") { Value = 0 };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@lastSeen") { Value = 0 };
			command.Parameters.Add(param);
			param = new SQLiteParameter("@idMovies") { Value = idMovies };
			command.Parameters.Add(param);

			executeCommandThreadSafe(command);
		}
	} 
}
