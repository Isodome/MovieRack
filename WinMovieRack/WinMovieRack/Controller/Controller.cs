using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.GUI;
using WinMovieRack.Model;

namespace WinMovieRack.Controller {
	public class Controller {

		private WinMovieRack.GUI.GUI gui;
		public SQLiteConnector db;
		private App app;

		private ImdbBrowserController browserController;
		private MainWindowController windowController;
		private DetailsViewController detailsViewController;

		public Controller(App app) {
			this.app = app;
			this.app.Exit += aboutToExit;
			initializeModel();
			initializeGUI();
			

			imdbMovieParserMaster parserMaster;
			parserMaster = new imdbMovieParserMaster(477348);
			parserMaster.setFinalizeFunction(this.filmFinished);
			//ThreadsMaster.getInstance().addJobMaster(parserMaster);
		}

		private void initializeGUI() {

			browserController = new ImdbBrowserController(this);
			IMDBBrowser browser = new IMDBBrowser(browserController);
			browserController.setBrowser(browser);

			windowController = new MainWindowController();
			MainWindow mw = new MainWindow(windowController);
			windowController.setMainWindow(this, mw);

			detailsViewController = new DetailsViewController(this, db);
			DetailsView dv = new DetailsView(detailsViewController);
			detailsViewController.setDetailsView(dv);

			gui = new WinMovieRack.GUI.GUI(this, mw, browser, dv);
		}

		private void initializeModel() {
			db = new SQLiteConnector();
			db.initDb();
		}


		public void changeToView(View view) {
			//Inform specific controllers
			switch (view) {
				case View.ACTORS_VIEW:
					break;
				case View.DETAILS_VIEW:
					break;
				case View.IMDB_BROWSER:
					browserController.activated();
					break;
				case View.LIST_VIEW:
					break;
			}
			gui.changeToView(view);
		}

		void filmFinished(ThreadJobMaster sender) {
			ImdbMovie p = ((imdbMovieParserMaster)sender).movieData;
			/*
			if (mainWindow.detailsView != null) {
				Dispatcher.BeginInvoke(new Action(() => {
					MovieRackListBoxItem boxItem = new MovieRackListBoxItem();
					boxItem.labelMovieTitle.Content = p.title;
					boxItem.labelMovieYear.Content = p.year;
					boxItem.labelMovieEditable.Content = (((float)p.imdbRating) / 10.0).ToString() + " ( " + p.imdbRatingVotes + " votes)";
					BitmapSource destination;
					IntPtr hBitmap = p.poster.GetHbitmap();
					BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions();
					destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, sizeOptions);
					destination.Freeze();
					boxItem.imageMovie.Source = destination;
					mainWindow.detailsView.addMoviesListBoxItem(boxItem);
				}));
			}
			  */

		}

		public void aboutToExit(object sender, ExitEventArgs e) {
			db.closeConnection();
			Application.Current.Shutdown();
		}
	}
}
