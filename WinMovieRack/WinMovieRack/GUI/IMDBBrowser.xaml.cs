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
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Model;

namespace WinMovieRack.GUI {
	/// <summary>
	/// Interaction logic for IMDBBrowser.xaml
	/// </summary>
	public partial class IMDBBrowser : UserControl {

		public string lastURL;
		public const string homeURL = "http://www.imdb.com";


		public IMDBBrowser() {
			InitializeComponent();
			this.imdbWebBrowser.Navigate(homeURL);
		}

		private void homeButton_Click(object sender, RoutedEventArgs e) {
			this.imdbWebBrowser.Navigate(homeURL);
			
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
			if (!Regex.Match(url, @"imdb\.com").Success) {
				imdbWebBrowser.Navigate(lastURL);
				return;
			}
			setButton("add", false);
			lastURL = url;

			if (IMDBUtil.isMovieUrl(url)) {
				imdbMovieParserMaster parser = new imdbMovieParserMaster(IMDBUtil.getTitleIdFromUrl(url));
				parser.setFinalizeFunction(this.updateActionButton);
				ThreadsMaster.getInstance().addJobMaster(parser);
			} else if (IMDBUtil.isNameURL(url)) {
				ConcurrentIMDBNameParser parser = new ConcurrentIMDBNameParser(IMDBUtil.getNameIdFromUrl(url));
				parser.setFinalizeFunction(this.updateActionButton);
				ThreadsMaster.getInstance().addJobMaster(parser);
			} else {
				updateActionButton(null);
			}
		}
		private void updateActionButton(ThreadJobMaster sender) {
			if (sender != null) {
				if (sender is ConcurrentIMDBNameParser) {
					setButton("Add " + ((ConcurrentIMDBNameParser)sender).person.name + " to Database", true);
				} else if (sender is imdbMovieParserMaster) {
					setButton("Add " + ((imdbMovieParserMaster)sender).movieData.title + " to Database", true);
				}
			} else {
				setButton("add", false);
			}
		}


		private void setButton(string newContent, bool active) {

			Dispatcher.BeginInvoke(new Action(() =>
                    {
						actionButton.IsEnabled = active;
						actionButton.Content = newContent;
                    }));
		}
	}
}
