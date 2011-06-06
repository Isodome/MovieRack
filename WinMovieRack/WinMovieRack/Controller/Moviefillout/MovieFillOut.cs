using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;

namespace WinMovieRack.Controller.Moviefillout {

	public class MovieFillOut {

		private Movie movie;
		private SQLiteConnector db;

		private int persons;
		Thread t;

		public MovieFillOut(Movie movie, SQLiteConnector db) {
			this.movie = movie;
			this.db = db;
			persons = 0;
		}

		public void startFillout() {
			 t = new Thread(new ThreadStart(parseNames));
		}

		public void parseNames() {
			foreach (uint cur in movie.imdbMovie.directors) {
				if (!db.testAndSetPerson(cur)) {
					startParse(cur);
				}
			}
			foreach (uint cur in movie.imdbMovie.writers) {
				if (!db.testAndSetPerson(cur)) {
					startParse(cur);
				}
			}
			foreach (Tuple<uint, string> cur in movie.imdbMovie.roles) {
				if (!db.testAndSetPerson(cur.Item1)) {
					startParse(cur.Item1);
				}
			}
		}
		private void startParse(uint i) {
			ConcurrentIMDBNameParser p = new ConcurrentIMDBNameParser(i);
			p.setFinalizeFunction(parseFinished);
			lock (this) {
				ThreadsMaster.getInstance().addJobMaster(p);
				persons++;
			}
		}

		public void parseFinished(ThreadJobMaster sender) {
			ConcurrentIMDBNameParser p = (ConcurrentIMDBNameParser)sender;
			lock (this.movie.persons) {
				this.movie.persons.Add(p.person);
			}
			t.Join();
			lock (this) {
				persons--;
				if (persons != 0) {
					return;
				}
			}
			insertInDB();
		}

		private void insertInDB() {
			
		}

	}
}
