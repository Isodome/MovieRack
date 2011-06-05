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
using WinMovieRack.Controller;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for Movies.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        private UIElement current;
        DetailsViewActorPanel actorPanel;

		DetailsViewController controller;

        public DetailsView(DetailsViewController dvc)
        {
            InitializeComponent();
			this.controller = dvc;
        }

        public void addMoviesListBoxItem(WinMovieRack.GUI.MovieRackListBoxItem item)
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
            int itemID = selectetItem.getdBID();
            if (selectetItem.getIsMovie())
            {
                //Get Info from MovieTable
            }
            else
            {
                //Get Info from PersonTable
            }
        }

        private void posterTitle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BigPicture bigPicture = new BigPicture();
            bigPicture.Show();
            bigPicture.bigPicture.Source=posterTitle.Source; //Only Debug, should load originalPicture
        }

    }
}
