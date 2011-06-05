using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;

namespace WinMovieRack.Controller {

	public class ImdbBrowserController {

		private IMDBBrowser browser;
		private Controller controller;

		public void setBrowser(Controller c ,IMDBBrowser b) {
			this.browser = b;
			this.controller = c;
		}

	}
}
