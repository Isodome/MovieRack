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
using WinMovieRack.GUI;
using WinMovieRack.Model;
using WinMovieRack.Controller;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for Movies.xaml
    /// </summary>
    public partial class DetailsView : System.Windows.Controls.UserControl
    {
        private UIElement current;
        DetailsViewActorPanel actorPanel;
        BigPicture bigPicture;

        DetailsViewController controller;
        List<MRListData> movieList;

        public DetailsView(DetailsViewController dvc)
        {
            InitializeComponent();
            this.controller = dvc;
        }

        public void addMoviesListBoxItem(MovieRackListBoxItem item)
        {
            listBoxMovies.Items.Add(item);
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            actorPanel = new DetailsViewActorPanel();
            changeView(actorPanel);
        }

        private void changeView(UIElement newView)
        {
            tabControl.Children.Remove(current);
            Grid.SetColumn(newView, 2);
            Grid.SetRow(newView, 2);
            Grid.SetColumnSpan(newView, 1);
            tabControl.Children.Add(newView);
            current = newView;
        }

        private void listBoxMovies_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            MovieRackListBoxItem selectetItem = (MovieRackListBoxItem)listBoxMovies.SelectedItem;
            int itemID = selectetItem.itemID;

        }

        private void posterTitle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bigPicture = new BigPicture();
            bigPicture.bigPicture.Source = posterTitle.Source; //Only Debug, should load originalPicture


            Point origin = new Point(0, 0);
            Point screenOrigin = posterTitle.PointToScreen(origin);
            bigPicture.setOrigin(posterTitle.Height, posterTitle.Width, screenOrigin.X, screenOrigin.Y);

            bigPicture.fadeIn();
        }

    }
}
