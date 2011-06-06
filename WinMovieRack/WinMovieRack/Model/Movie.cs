using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Controller.Parser.imdbMovieParser;

namespace WinMovieRack.Model {

	public class Movie {
		private uint uniqueID;
		public ImdbMovie imdbMovie;
		public List<ImdbPerson> persons;

		public Movie(ImdbMovie imdbMovie) {
			this.imdbMovie = imdbMovie;
		}

		public void imdbParseFinished(ThreadJobMaster sender) {
			imdbMovieParserMaster parser = (imdbMovieParserMaster)sender;
			this.imdbMovie = parser.movieData;
		}

	}
}
