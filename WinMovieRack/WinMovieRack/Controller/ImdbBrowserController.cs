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
		private bool firstLoad = true;

		public ImdbBrowserController(Controller c) {
			this.controller = c;
		}


		public void setBrowser(IMDBBrowser b) {
			this.browser = b;
		}

		public void activated() {

			if (firstLoad) {
				firstLoad = false;
				browser.goToHome();
			}
		}

		public void insertMovieInDB(uint imdbid) {
			MovieFillOut fillout = new MovieFillOut(imdbid);
			fillout.startFillout();
		}

		public void insertPersonInDB(ConcurrentIMDBNameParser parser) {

		}

	}
}
