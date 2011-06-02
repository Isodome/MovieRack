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
                //threadsMaster.addJobMaster(parserMaster);
                parserMaster = new imdbMovieParserMaster(449088);
               // threadsMaster.addJobMaster(parserMaster);
				threadsMaster.addJobMaster(new ConcurrentIMDBNameParser(4695));
            }
        
    }
}
