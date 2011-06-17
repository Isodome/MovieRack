using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller;
using WinMovieRack.Model;

namespace WinMovieRack.Controller.Moviefillout {

	public delegate void CallBack (Movie m);
	public class MovieFillOut {


		private uint imdbid;
		private Movie movie;

		public MovieFillOut(uint imdbid) {
			this.imdbid = imdbid;
		}

		public void startFillout() {
			int idMovies;
			SQLiteConnector.db.testAndSetMovies(imdbid, out idMovies);
			ConcurrentImdbMovieParser p = new ConcurrentImdbMovieParser(imdbid);
			p.setFinalizeFunction(this.parseNames);
			ThreadsMaster.getInstance().addJobMaster(p);

		}

		public void parseNames(ThreadJobMaster sender) {
				// get parsed movie data;
			ConcurrentImdbMovieParser parser = sender as ConcurrentImdbMovieParser;
			this.movie = new Movie(parser.movieData);

			List<uint> IDsToTest = new List<uint>();

			int idPerson;
			SQLiteConnector.db.beginTransaction();
			
			IDsToTest.AddRange(movie.imdbMovie.directors);
			IDsToTest.AddRange(movie.imdbMovie.writers);
			foreach (Tuple<uint, string> cur in movie.imdbMovie.roles) {
					IDsToTest.Add(cur.Item1);
			}
			foreach (Award a in movie.imdbMovie.awards) {
				IDsToTest.AddRange(a.persons);
			}
			bool[] contains =SQLiteConnector.db.testAndSetPersons(IDsToTest);
			int i = 0;
			foreach (uint id in IDsToTest) {
				if (!contains[i]) {
					startParse(id);
				}
			}
			SQLiteConnector.db.updateMovieData(this.movie);
			SQLiteConnector.db.endTransaction();
		}

		private void startParse(uint imdbID) {
			ConcurrentIMDBNameParser p = new ConcurrentIMDBNameParser(imdbID);
			p.setFinalizeFunction(parseFinished);
			ThreadsMaster.getInstance().addJobMaster(p);
		}

		public void parseFinished(ThreadJobMaster sender) {
			ConcurrentIMDBNameParser p = sender as ConcurrentIMDBNameParser;
			SQLiteConnector.db.updateImdbPerson(p.person);
		}


	}
}
