using System.Windows;



namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

            void App_StartUp(object sender, StartupEventArgs e)
            {
				new WinMovieRack.Controller.Controller(this);
				
            }   
    }
}
