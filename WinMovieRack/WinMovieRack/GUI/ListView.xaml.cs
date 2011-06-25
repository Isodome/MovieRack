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
using WinMovieRack.Controller;
using System.Runtime.InteropServices;
using WinMovieRack.Model;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : System.Windows.Controls.UserControl
    {
        ListViewController controller;
        GUIMovie movieDetails;
        public ListView(ListViewController lvc)
        {
            InitializeComponent();
            this.controller = lvc;
        }

        private void movieList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            movieDetails = (GUIMovie)movieList.SelectedItem;
            controller.changeToMovieView((GUIMovie)movieList.SelectedItem);
            tabControl.SelectedIndex = 1;
        }

        private void movieListBoxContext_Opened(object sender, RoutedEventArgs e)
        {
            contextMenueTitle.Header = movieDetails.title;
            Uri uri = new Uri(PictureHandler.getMoviePosterPath(movieDetails.dbId, PosterSize.TINY));
            contextMoviePoster.Source = new System.Windows.Media.Imaging.BitmapImage(uri);
        }

        private void menuItemSeen_Click(object sender, RoutedEventArgs e)
        {
            Seen seen = new Seen(controller, movieDetails.dbId);
            seen.ShowDialog();
        }
    }
}

