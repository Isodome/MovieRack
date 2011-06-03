using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.GUI;

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
				imdbMovieParserMaster p = (imdbMovieParserMaster)sender;
                if (mainWindow.detailsView != null)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MovieRackListBoxItem boxItem = new MovieRackListBoxItem();
                        boxItem.setListBoxTitle(p.title);
                        boxItem.setYearCharakter(p.year.ToString());
                        boxItem.setEditableAge(p.imdbRating.ToString());
                        mainWindow.detailsView.addMoviesListBoxItem(boxItem);
                    }));
                }
				foreach(Tuple<uint,string> t in p.roles) {
					ThreadsMaster.getInstance().addJobMaster(new ConcurrentIMDBNameParser(t.Item1));
				}
			}
        
    }
}
