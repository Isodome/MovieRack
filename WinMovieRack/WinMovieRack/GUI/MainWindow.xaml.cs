using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinMovieRack.Resources.Localization.MainWindow;
using WinMovieRack.Model;
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.GUI;
using WinMovieRack.Controller.ThreadManagement;
using System.Threading;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIElement current;
		private MainWindowController controller;

        public MainWindow(MainWindowController c)
        {
            InitializeComponent();
			this.controller = c;
        }

        void setLocalization()
        {
            fileMenuEntry.Header = MainWindowStrings.FileKey;
        }

        private void moviesMenuEntry_Clicked(object sender, RoutedEventArgs e)
        {
			controller.shouldChangeView(View.DETAILS_VIEW);
        }

        public void changeView(UIElement newView)
        {
            mainGrid.Children.Remove(current);
            Grid.SetColumn(newView, 0);
            Grid.SetRow(newView, 2);
            Grid.SetColumnSpan(newView, 3);
            mainGrid.Children.Add(newView);
            current = newView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

		private void imdbMenuEntry_Click(object sender, RoutedEventArgs e) {
			controller.shouldChangeView(View.IMDB_BROWSER);
		}

        private void todoMenuEntry_Click(object sender, RoutedEventArgs e) {
            controller.shouldChangeView(View.TODO_LIST);
        }

        private void actorsMenuEntry_Click(object sender, RoutedEventArgs e)
        {
            controller.shouldChangeView(View.ACTORS_VIEW);
        }

		private void importMenuItem_Click(object sender, RoutedEventArgs e) {
			ImportWindow i = new ImportWindow();
			i.ShowDialog();
		}

		public void setProgressIndicating(bool i) {
			Action a = new Action(() => {
				this.progressIndicator.Visibility = i ? Visibility.Visible : Visibility.Hidden;
			}) ;
			Dispatcher.BeginInvoke(a); 
				
		}

        private void listMenuEntry_Click(object sender, RoutedEventArgs e)
        {
            controller.shouldChangeView(View.LIST_VIEW);
        }
    }
}
