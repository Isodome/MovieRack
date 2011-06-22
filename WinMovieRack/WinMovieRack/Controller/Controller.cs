using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using WinMovieRack.Controller.Parser.BoxOffice;
using System.Net;
namespace WinMovieRack.Controller {
	public class Controller {

		private WinMovieRack.GUI.GUI gui;
		public SQLiteConnector db;
		public static Controller controller;
		private App app;

		private ImdbBrowserController browserController;
		private MainWindowController windowController;
		private DetailsViewController detailsViewController;
        private ActorsViewController actorsViewController;
        private ListViewController listViewController;

		public Controller(App app) {
			this.app = app;
			this.app.Exit += aboutToExit;
			initializeModel();
			initializeGUI();
			controller = this;
			ServicePointManager.DefaultConnectionLimit = 65000;
			
		}

		public void func(ConcThreadJobMaster sender) {
			((ConcurrentImdbMovieParser)sender).movieData.printToConsole();
		}

		private void initializeGUI() {

			browserController = new ImdbBrowserController(this);
			IMDBBrowser browser = new IMDBBrowser(browserController);
			browserController.setBrowser(browser);

			windowController = new MainWindowController();
			MainWindow mw = new MainWindow(windowController);
            mw.Width = 1024;
            mw.Height = 600;
			windowController.setMainWindow(this, mw);

			detailsViewController = new DetailsViewController(this, db);
			DetailsView dv = new DetailsView(detailsViewController);
			detailsViewController.setDetailsView(dv);

            actorsViewController = new ActorsViewController(this, db);
            ActorsView av = new ActorsView(actorsViewController);
            actorsViewController.setActorsView(av);
           
            listViewController = new ListViewController(this, db);
            ListView lv = new ListView(listViewController);
            listViewController.setListView(lv);

			gui = new WinMovieRack.GUI.GUI(this, mw, browser, dv, av, lv);
		}

		private void initializeModel() {
			db = new SQLiteConnector();
			db.initDb();
			PictureHandler.initialize();
		}


		public void changeToView(View view) {
			//Inform specific controllers
			switch (view) {
				case View.ACTORS_VIEW:
                    actorsViewController.loadCompleteActorsList();
					break;
				case View.DETAILS_VIEW:
                    detailsViewController.loadCompleteMovieList();
					break;
				case View.IMDB_BROWSER:
					browserController.activated();
					break;
				case View.LIST_VIEW:

					break;
			}
			gui.changeToView(view);
		}


		public void aboutToExit(object sender, ExitEventArgs e) {
			db.closeConnection();
			windowController.close();
			ThreadsMaster.getInstance().waitToFinish();
			Application.Current.Shutdown();
		}

		internal void setProgressIndicator(bool p) {
			windowController.setProgressIndicator(p);
		}
	}
}
