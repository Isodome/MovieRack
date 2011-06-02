using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WinMovieRack.Controller;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

            void App_StartUp(object sender, StartupEventArgs e)
            {
                ThreadsMaster threadsMaster = new ThreadsMaster(4);
                imdbMovieParserMaster parserMaster;
                parserMaster = new imdbMovieParserMaster(477348);
                threadsMaster.addJobMaster(parserMaster);
                parserMaster = new imdbMovieParserMaster(449088);
                threadsMaster.addJobMaster(parserMaster); 
            }
        
    }
}
