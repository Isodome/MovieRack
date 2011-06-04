using System;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.GUI;
using WinMovieRack.Model;

namespace WinMovieRack.Controller {
	public class Controller {
		private WinMovieRack.GUI.GUI gui;
		private App app;

		public Controller(App app) {
			this.app = app;
			gui = new WinMovieRack.GUI.GUI(this);

			imdbMovieParserMaster parserMaster;
			parserMaster = new imdbMovieParserMaster(477348);
			parserMaster.setFinalizeFunction(this.filmFinished);
			ThreadsMaster.getInstance().addJobMaster(parserMaster);
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
	}
}
