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

			ConcurrentImdbMovieParser p = new ConcurrentImdbMovieParser(imdbid);
			p.setFinalizeFunction(this.parseNames);
			ThreadsMaster.getInstance().addJobMaster(p);
		}

		public void parseNames(ThreadJobMaster sender) {

			ConcurrentImdbMovieParser parser = sender as ConcurrentImdbMovieParser;
			this.movie = new Movie(parser.movieData);

			int idPerson;
			SQLiteConnector.db.beginTransaction();

			foreach (uint cur in movie.imdbMovie.directors) {
				if (!SQLiteConnector.db.testAndSetPerson(cur, out idPerson)) {
					startParse(cur, idPerson);
				}
			}
			foreach (uint cur in movie.imdbMovie.writers) {
				if (!SQLiteConnector.db.testAndSetPerson(cur, out idPerson)) {
					startParse(cur, idPerson);
				}
			}
			foreach (Tuple<uint, string> cur in movie.imdbMovie.roles) {
				if (!SQLiteConnector.db.testAndSetPerson(cur.Item1, out idPerson)) {
					startParse(cur.Item1, idPerson);
				}
			}
			foreach (Award a in movie.imdbMovie.awards) {
				foreach (uint p in a.persons) {
					if (!SQLiteConnector.db.testAndSetPerson(p, out idPerson)) {
						startParse(p, idPerson);
					}
				}
			}
			SQLiteConnector.db.endTransaction();
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
			SQLiteConnector.db.updateImdbPerson(p.person);
		}

		public void insertIntoDB() {
			SQLiteConnector.db.insertMovieData(this.movie);
		}

	}
}
