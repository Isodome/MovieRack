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
using WinMovieRack.Controller.ThreadManagement;
using System.Text.RegularExpressions;

namespace WinMovieRack.GUI {
	/// <summary>
	/// Interaction logic for IMDBBrowser.xaml
	/// </summary>
	public partial class IMDBBrowser : UserControl {

		public string lasURL;


		public IMDBBrowser() {
			InitializeComponent();
		}

		private void homeButton_Click(object sender, RoutedEventArgs e) {
			this.imdbWebBrowser.Navigate("http://www.imdb.com");
			
		}

		private void backButton_Click(object sender, RoutedEventArgs e) {
			this.imdbWebBrowser.GoBack();
		}

		private void forwardButton_Click(object sender, RoutedEventArgs e) {
			this.imdbWebBrowser.GoForward();
		}

		private void actionButton_Click(object sender, RoutedEventArgs e) {
			
		}


		private void imdbWebBrowser_Navigating(object sender, NavigatingCancelEventArgs e) {
			string url = e.Uri.ToString();

			if (!Regex.Match(url, @"imdb.com").Success) {
				imdbWebBrowser.Navigate(lasURL);
			} else {
				lasURL = url;
			}
			Console.WriteLine("Navigating: " + e.Uri);
		}


		private void imdbWebBrowser_LoadCompleted(object sender, NavigationEventArgs e) {
			Console.WriteLine("LoadedCOmplete: ");
		}



	}
}
