using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.GUI;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.Moviefillout;
using WinMovieRack.Model;

namespace WinMovieRack.Controller {

	public class ImdbBrowserController {

		private IMDBBrowser browser;
		private Controller controller;

		public ImdbBrowserController(Controller c) {
			this.controller = c;
		}


		public void setBrowser(IMDBBrowser b) {
			this.browser = b;
		}

		public void insertMovieInDB(imdbMovieParserMaster parser) {
			Movie m = new Movie(parser.movieData);
			ConcurrentMovieFillOut fillout = new ConcurrentMovieFillOut(m, controller.db);
			ThreadsMaster.getInstance().addJobMaster(fillout);
		}

		public void insertPersonInDB(ConcurrentIMDBNameParser parser) {

		}

	}
}
