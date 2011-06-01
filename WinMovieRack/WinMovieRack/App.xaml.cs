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
                imdbMovieParserMaster parserMaster = new imdbMovieParserMaster();
                ThreadsMaster threadsMaster = new ThreadsMaster();
                threadsMaster.addJobMaster(parserMaster);
            }
        
    }
}
