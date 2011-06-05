using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinMovieRack.Controller;


namespace WinMovieRack.GUI {
	class GUI {

		private WinMovieRack.Controller.Controller controller;

		private MainWindow mainWindow;
		private DetailsView detailsView;
		private IMDBBrowser imdbBrowser;


		public GUI(WinMovieRack.Controller.Controller c, MainWindow mw, IMDBBrowser browser, DetailsView dv) {
			this.controller = c;
			this.imdbBrowser = browser;
			this.mainWindow = mw;
			this.detailsView = dv;

			mainWindow = mw;
			mainWindow.Show();

		}

		public void changeToView(View view) {
			switch (view) {
				case View.ACTORS_VIEW:
					break;
				case View.DETAILS_VIEW:
					mainWindow.changeView(detailsView);
					break;
				case View.IMDB_BROWSER:
					mainWindow.changeView(imdbBrowser);
					break;
				case View.LIST_VIEW:
					break;
			}
		}


	}
}
