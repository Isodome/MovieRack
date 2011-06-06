using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;

namespace WinMovieRack.Controller.Moviefillout {
	class JobCheckPersonIsInDatabase : ThreadJob {

		private SQLiteConnector db;

		public List<uint> directors;
		public List<uint> writers;
		public List<Tuple<uint, string>> roles;

		public JobCheckPersonIsInDatabase(ImdbMovie movie, SQLiteConnector db) {
			this.directors = movie.directors;
			this.writers = movie.writers;
			this.roles = movie.roles;
			this.db = db;
		}

		public void run() {

		}
	}
}
