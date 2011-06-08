using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;

namespace WinMovieRack.Controller.Moviefillout {

	public delegate void CallBack (Movie m);
	public class MovieFillOut {

		private Movie movie;
		private SQLiteConnector db;
		private CallBack cb;

		private int persons;
		Thread t;

		public MovieFillOut(Movie movie, SQLiteConnector db, CallBack cb) {
			this.movie = movie;
			this.db = db;
			this.cb = cb;
			persons = 0;
		}

		public void startFillout() {
			 t = new Thread(new ThreadStart(parseNames));
			 t.Start();
		}

		public void parseNames() {
			bool newActor = false;
			foreach (uint cur in movie.imdbMovie.directors) {
				if (!db.testAndSetPerson(cur)) {
					startParse(cur);
					newActor = true;
				}
			}
			foreach (uint cur in movie.imdbMovie.writers) {
				if (!db.testAndSetPerson(cur)) {
					startParse(cur);
					newActor = true;
				}
			}
			foreach (Tuple<uint, string> cur in movie.imdbMovie.roles) {
				if (!db.testAndSetPerson(cur.Item1)) {
					startParse(cur.Item1);
					newActor = true;
				}
			}
			if (!newActor) {
				cb(this.movie);
			}
		}

		private void startParse(uint i) {
			ConcurrentIMDBNameParser p = new ConcurrentIMDBNameParser(i);
			p.setFinalizeFunction(parseFinished);
			Monitor.Enter(this);
			ThreadsMaster.getInstance().addJobMaster(p);
			persons++;
			Monitor.Exit(this);
		}

		public void parseFinished(ThreadJobMaster sender) {
			ConcurrentIMDBNameParser p = (ConcurrentIMDBNameParser)sender;
			lock (this.movie.persons) {
				this.movie.persons.Add(p.person);
			}
			t.Join();
			Monitor.Enter(this);
			persons--;
			if (persons != 0) {
				Monitor.Exit(this);
				return;
			}

			Monitor.Exit(this);
			cb(this.movie);
		}



	}
}
