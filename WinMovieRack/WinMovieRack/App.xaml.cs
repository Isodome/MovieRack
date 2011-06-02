using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser.imdbNameParser;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

            void App_StartUp(object sender, StartupEventArgs e)
            {
                ThreadsMaster threadsMaster = new ThreadsMaster();
                imdbMovieParserMaster parserMaster;
				parserMaster = new imdbMovieParserMaster(477348);
				parserMaster.setFinalizeFunction(this.filmFinished);
                threadsMaster.addJobMaster(parserMaster);


            }
			void filmFinished(ThreadJobMaster sender)
			{
				imdbMovieParserMaster p = (imdbMovieParserMaster)sender;
				foreach(Tuple<uint,string> t in p.roles) {
					ThreadsMaster.getInstance().addJobMaster(new ConcurrentIMDBNameParser(t.Item1));
				}
			}
        
    }
}
