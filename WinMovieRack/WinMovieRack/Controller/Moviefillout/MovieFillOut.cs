using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;
using System.Threading;
namespace WinMovieRack.Controller.Moviefillout {

	public class MovieFillOut {

		private Movie movie;
		private SQLiteConnector db;

		public MovieFillOut(Movie movie, SQLiteConnector db) {
			this.movie = movie;
			this.db = db;
		}

		public void startFillout() {
			Thread t = new Thread(new ThreadStart(parseNames));
		}

		public void parseNames() {
			foreach (uint cur in movie.imdbMovie.directors) {
				if (!true) {
					
				}
			}
			foreach (uint cur in movie.imdbMovie.writers) {
				if (!true) {

				}
			}
			foreach (Tuple<uint, string> cur in movie.imdbMovie.roles) {
				if (!true) {

				}
			}
		}



	}
}
