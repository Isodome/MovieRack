using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Controller;
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
			db.beginTransaction();
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
			foreach (Award a in movie.imdbMovie.awards) {
				foreach (uint p in a.persons) {
					if (!db.testAndSetPerson(p)) {
						startParse(p);
						newActor = true;
					}
				}
			}
			db.endTransaction();
			Finalizer f = new Finalizer();
			f.addFunction(this.insertIntoDB);
			ThreadsMaster.getInstance().addJobMaster(f);
		}

		private void startParse(uint imdbID) {
			ConcurrentIMDBNameParser p = new ConcurrentIMDBNameParser(imdbID);
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
		}

		public void insertIntoDB() {
			cb(this.movie);
		}

	}
}
