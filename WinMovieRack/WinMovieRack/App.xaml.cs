using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.GUI;
using System.Windows.Media.Imaging;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private MainWindow mainWindow;

            void App_StartUp(object sender, StartupEventArgs e)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
                ThreadsMaster threadsMaster = new ThreadsMaster();
                imdbMovieParserMaster parserMaster;
				parserMaster = new imdbMovieParserMaster(477348);
				parserMaster.setFinalizeFunction(this.filmFinished);
                threadsMaster.addJobMaster(parserMaster);

            }
			void filmFinished(ThreadJobMaster sender)
			{
				Movie p = ((imdbMovieParserMaster)sender).movieData;
                if (mainWindow.detailsView != null)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
						MovieRackListBoxItem boxItem = new MovieRackListBoxItem();
						boxItem.labelMovieTitle.Content = p.title;
						boxItem.labelMovieYear.Content = p.year;
						boxItem.labelMovieEditable.Content = (((float)p.imdbRating)/10.0).ToString() + " ( " + p.imdbRatingVotes + " votes)";
 						BitmapSource destination; 
						IntPtr hBitmap = p.poster.GetHbitmap(); 
						BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions(); 
						destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, sizeOptions); 
						destination.Freeze();
						boxItem.imageMovie.Source = destination;
                        mainWindow.detailsView.addMoviesListBoxItem(boxItem);
                    }));
                }
				foreach(Tuple<uint,string> t in p.roles) {
					ThreadsMaster.getInstance().addJobMaster(new ConcurrentIMDBNameParser(t.Item1));
				}
			}
        
    }
}
