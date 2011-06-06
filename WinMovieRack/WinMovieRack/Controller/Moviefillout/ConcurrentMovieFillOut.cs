using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;

namespace WinMovieRack.Controller.Moviefillout {

	delegate void needDownload(uint nameID);

	public class ConcurrentMovieFillOut : ThreadJobMaster {

		private ThreadJob checkPerson;

		public ConcurrentMovieFillOut(Movie movie, SQLiteConnector db) {
			this.checkPerson = new JobCheckPersonIsInDatabase(movie.imdbMovie, db);
		}

		public override bool hasFinished(ThreadJob job) {
			return false;
		}
	}
}
