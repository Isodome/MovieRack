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
using System.Windows.Shapes;
using Microsoft.Win32;
using WinMovieRack.Controller.Moviefillout;
using System.IO;
using System.Threading;
namespace WinMovieRack.GUI {
	/// <summary>
	/// Interaction logic for ImportWindow.xaml
	/// </summary>
	public partial class ImportWindow : Window {
		public ImportWindow() {
			InitializeComponent();
			setLocalization();
			fileExists();
		}

		private void setLocalization() {
			this.header.Content = "Select source for import:";

			this.cancelButton.Content = "Cancel";
			this.importButton.Content = "Import Now";
			this.doLaterButton.Content = "Add to ToDo List";
		}

		private void imdbidsBrowseButton_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.DefaultExt = ".txt";
			dlg.Multiselect = false;

			dlg.Filter = string.Empty;
			Nullable<bool> result = dlg.ShowDialog();
			if (result == true) {
				imdbidsFileSource.Text = dlg.FileName;
			}
		}

		private void importButton_Click(object sender, RoutedEventArgs e) {
			TabItem current = importTypeSelector.SelectedItem as TabItem;
			if (current == tabByImdbIds) {
				importByImdbIds(false);
			}
		}


        private void todoButton_Click(object sender, RoutedEventArgs e) {
			TabItem current = importTypeSelector.SelectedItem as TabItem;
			if (current == tabByImdbIds) {
                importByImdbIds(true);
			}
		}


		private void importByImdbIds(Boolean importToTodoList) {
			string path = imdbidsFileSource.Text;
			if (!File.Exists(path)) {
				MessageBox.Show("The File you selected does't exist");
			} else {
				FileInfo file = new FileInfo(path);
				ImdbIdsImporter importer = new ImdbIdsImporter(file.OpenText().ReadToEnd());
				Thread t = new Thread(new ParameterizedThreadStart(importer.import));
                t.Start(importToTodoList);
				this.Close();
			}
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void imdbidsFileSource_TextChanged(object sender, TextChangedEventArgs e) {
			fileExists();
		}

		private bool fileExists() {
			bool fileExists = File.Exists(imdbidsFileSource.Text);
			this.importButton.IsEnabled = fileExists;
			this.doLaterButton.IsEnabled = fileExists;
			return fileExists;
		}
	}
}
