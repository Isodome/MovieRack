using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace WinMovieRack.Model {
	public partial class SQLiteConnector {

		public void insertMovieData(Movie m) {
			insertImdbMovie(m.imdbMovie);
			foreach (ImdbPerson person in m.persons) {
				updateImdbPerson(person);
			}

			int idMovies = getIdMoviesByImdbId(m.imdbMovie.imdbID);
			if (idMovies > -1) {
				foreach (Tuple<uint, string> t in m.imdbMovie.roles) {
					insertRole(t.Item1, t.Item2, (uint)idMovies);
				}
				foreach (uint id in m.imdbMovie.directors) {

				}
				foreach (uint id in m.imdbMovie.writers) {

				}
			}
			if (m.imdbMovie.poster != null) {
				PictureHandler.saveMoviePoster(m.imdbMovie.poster, idMovies);
			}

		}
		private void updateImdbPerson(ImdbPerson person) {

			int idPerson = getIdPersonByImdbId(person.imdbID);
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
			param = new SQLiteParameter("@Birthday") { Value = 0 };
			command.Parameters.Add(param);
			//TODO
			param = new SQLiteParameter("@Deathday") { Value = 0 };
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
			param = new SQLiteParameter("@idPerson") { Value = idPerson };
			command.Parameters.Add(param);

			executeCommandThreadSafe(command);
			if (person.image != null) {
				PictureHandler.savePersonPortrait(person.image, idPerson);
			}
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

				executeCommandThreadSafe(command);
			}
		}



		private void insertImdbMovie(ImdbMovie movie) {
			SQLiteCommand command = new SQLiteCommand(connection);

			command.CommandText = "INSERT INTO Movies (title, runtime, plot, originalTitle, " +
				"imdbID, imdbRating, imdbRatingVotes, imdbTop250, metacriticsReviewRating, " +
				"metacriticsUsersRating, rottenTomatoesAudience, tomatometer, personalRating, " +
				"year, boxofficeWorldwide, boxofficeAmerica, boxofficeForeign, boxofficeFirstWeekend, " +
				"rangFirstWeekend, rankAllTime, weeksInCinema, otherWins, otherNominations, notes, seenCount, " +
				"TVSeries, lastSeen)" +
				"VALUES(@title, @runtime, @plot, @originalTitle, " +
				"@imdbID, @imdbRating, @imdbRatingVotes, @imdbTop250, @metacriticsReviewRating, " +
				"@metacriticsUsersRating, @rottenTomatoesAudience, @tomatometer, @personalRating, " +
				"@year, @boxofficeWorldwide, @boxofficeAmerica, @boxofficeForeign, @boxofficeFirstWeekend, " +
				"@rangFirstWeekend, @rankAllTime, @weeksInCinema, @otherWins, @otherNominations, @notes, @seenCount, " +
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
	} 
}
