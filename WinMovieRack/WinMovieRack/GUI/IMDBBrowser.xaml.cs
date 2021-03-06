﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.GUI {
	/// <summary>
	/// Interaction logic for IMDBBrowser.xaml
	/// </summary>
	public partial class IMDBBrowser : UserControl {

		public string lastURL;
		public const string homeURL = "http://www.imdb.com";
		private ImdbBrowserController controller;
		private ConcThreadJobMaster lastParser;


		public IMDBBrowser(ImdbBrowserController contr) {
			InitializeComponent();
			this.controller = contr;
		}

		public void goToHome() {
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
			if (lastParser != null) {
				if (lastParser is ConcurrentIMDBNameParser) {
					controller.insertPersonInDB((ConcurrentIMDBNameParser)lastParser);
				} else if (lastParser is ConcurrentImdbMovieParser) {
					controller.insertMovieInDB(IMDBUtil.getTitleIdFromUrl(lastURL));
				}
			}
			lastParser = null;
		}
		

		private void imdbWebBrowser_Navigating(object sender, NavigatingCancelEventArgs e) {
			string url = e.Uri.ToString();
			if (!Regex.Match(url, @"imdb\.com").Success) {
				imdbWebBrowser.Navigate(lastURL);
				return;
			}
			setButton("add", false);
			lastParser = null;
			lastURL = url;

			if (IMDBUtil.isMovieUrl(url)) {
				ConcurrentImdbMovieParser parser = new ConcurrentImdbMovieParser(IMDBUtil.getTitleIdFromUrl(url));
				parser.setFinalizeFunction(this.updateActionButton);
				ThreadsMaster.getInstance().addVeryVeryImportantThreadMaster(parser);
			} else if (IMDBUtil.isNameURL(url)) {
				ConcurrentIMDBNameParser parser = new ConcurrentIMDBNameParser(IMDBUtil.getNameIdFromUrl(url));
				parser.setFinalizeFunction(this.updateActionButton);
				ThreadsMaster.getInstance().addVeryVeryImportantThreadMaster(parser);
			} else {
				updateActionButton(null);
			}
		}

		private void updateActionButton(ConcThreadJobMaster sender) {
			if (sender != null) {
				if (sender is ConcurrentIMDBNameParser) {
					setButton("Add '" + ((ConcurrentIMDBNameParser)sender).person.name + "' to Database", true);
				} else if (sender is ConcurrentImdbMovieParser) {
					setButton("Add '" + ((ConcurrentImdbMovieParser)sender).movieData.title + "' to Database", true);
				}
				lastParser = sender;
			} else {
				setButton("add", false);
			}
		}


		private void setButton(string newContent, bool active) {

			Dispatcher.BeginInvoke(new Action(() => {
						actionButton.IsEnabled = active;
						actionButton.Content = newContent;
					}));
		}
	}
}
