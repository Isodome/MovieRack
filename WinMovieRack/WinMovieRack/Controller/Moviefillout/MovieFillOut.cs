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

		Thread t;

		public MovieFillOut(Movie movie, SQLiteConnector db, CallBack cb) {
			this.movie = movie;
			this.db = db;
			this.cb = cb;
		}

		public void startFillout() {
			 t = new Thread(new ThreadStart(parseNames));
			 t.Start();
		}

		public void parseNames() {
			int idPerson;
			db.beginTransaction();

			foreach (uint cur in movie.imdbMovie.directors) {
				if (!db.testAndSetPerson(cur, out idPerson)) {
					startParse(cur, idPerson);
				}
			}
			foreach (uint cur in movie.imdbMovie.writers) {
				if (!db.testAndSetPerson(cur, out idPerson)) {
					startParse(cur, idPerson);
				}
			}
			foreach (Tuple<uint, string> cur in movie.imdbMovie.roles) {
				if (!db.testAndSetPerson(cur.Item1, out idPerson)) {
					startParse(cur.Item1, idPerson);
				}
			}
			foreach (Award a in movie.imdbMovie.awards) {
				foreach (uint p in a.persons) {
					if (!db.testAndSetPerson(p, out idPerson)) {
						startParse(p, idPerson);
					}
				}
			}
			db.endTransaction();
			FunctionCaller f = new FunctionCaller();
			f.addFunction(this.insertIntoDB);
			f.isWaiting = true;
			ThreadsMaster.getInstance().addJobMaster(f);
		}

		private void startParse(uint imdbID, int idPerson) {
			ConcurrentIMDBNameParser p = new ConcurrentIMDBNameParser(imdbID);
			p.person.idPerson = idPerson;
			p.setFinalizeFunction(parseFinished);
			Monitor.Enter(this);
			ThreadsMaster.getInstance().addJobMaster(p);
			Monitor.Exit(this);
		}

		public void parseFinished(ThreadJobMaster sender) {
			ConcurrentIMDBNameParser p = sender as ConcurrentIMDBNameParser;
			db.updateImdbPerson(p.person);
		}

		public void insertIntoDB() {
			cb(this.movie);
		}

	}
}
