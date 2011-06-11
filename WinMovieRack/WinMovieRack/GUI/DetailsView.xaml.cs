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
using System.IO;
using System.Collections;


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
        GUIMovie movieDetails;
        DetailsViewController controller;
        public DetailsView(DetailsViewController dvc)
        {
            InitializeComponent();
            this.controller = dvc;
        }

        public void addMoviesListBoxItem(MovieRackListBoxItem item)
        {
            listBoxMovies.Items.Add(item);
        }

        public void gernerateCastListBox(MovieRackListBoxItem item)
        {
            castListBox.Items.Add(item);
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

        private void listBoxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieRackListBoxItem selectetItem = (MovieRackListBoxItem)listBoxMovies.SelectedItem;
            if (selectetItem != null)
            {
                getMovieDetails(selectetItem.itemID);
                setMovieDetails(movieDetails);
                controller.loadActorList(selectetItem.itemID);
            }
        }

        private void posterTitle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bigPicture = new BigPicture();
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movieDetails.dbId, PosterSize.FULL));
            posterBitmap.EndInit();
            bigPicture.bigPicture.Source = posterBitmap;


            Point origin = new Point(0, 0);
            Point screenOrigin = posterTitle.PointToScreen(origin);
            bigPicture.setOrigin(posterTitle.Source.Height, posterTitle.Source.Width, screenOrigin.X, screenOrigin.Y);

            bigPicture.fadeIn();
        }


        public void resetMovieList()
        {
            listBoxMovies.Items.Clear();
        }
        public void resetActorList()
        {
            castListBox.Items.Clear();
        }
        private void getMovieDetails(int itemID)
        {
            this.movieDetails = controller.getGUIMovie(itemID);
        }

        public void sortListBox()
        {
            //  System.ComponentModel.SortDescription test = new System.ComponentModel.SortDescription("Content.title", System.ComponentModel.ListSortDirection.Descending);
            //  Console.Write(test.PropertyName);
            //  listBoxMovies.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Content", System.ComponentModel.ListSortDirection.Descending));
            // ArrayList arrList;
            //  arrList = ArrayList.Adapter(listBoxMovies.Items);
            //  arrList.Sort(new ListComparerTest()); 
        }

        private void setMovieDetails(GUIMovie movieDetails)
        {
            this.movieDetails = movieDetails;
            yearLabel.Content = "(" + movieDetails.year + ")";
            movieTitleLabel.Text = movieDetails.title;
            orgialTitleLabel.Content = movieDetails.originalTitle;
            imdbRating.Content = movieDetails.imdbRating + "/10";
            imdbVotes.Content = "(" + movieDetails.imdbRatingVotes + " Votes)";
            if (movieDetails.imdbTop250 > 0)
            {
                top250.Content = movieDetails.imdbTop250;
            }
            else
            {
                top250.Content = "N/A";
            }
            plot.Text = movieDetails.plot;
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movieDetails.dbId, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterTitle.Source = posterBitmap;
            runtime.Content = movieDetails.runtime;

        }

        private void castListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieRackListBoxItem selectetItem = (MovieRackListBoxItem)castListBox.SelectedItem;
            if (selectetItem != null)
            {
                GUIPerson personDetails = controller.getGUIPerson(selectetItem.itemID);
                setPersonDetails(personDetails);
            }
        }
        private void setPersonDetails(GUIPerson personDetails)
        {
            personName.Text = personDetails.Name;
            locationOfBirth.Content = personDetails.CityofBirth;
            oscars.Content = personDetails.OscarWins;
            lifetimeGross.Content = personDetails.lifetimeGross;
            averageBoxoffice.Content = personDetails.boxofficeAverage;

            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(personDetails.dbID, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            actorPoster.Source = posterBitmap;
        }

        public System.Windows.Controls.ListBox getlistBoxMovies()
        {
            return listBoxMovies;
        }

        public System.Windows.Controls.ListBox getcastListBox()
        {
            return castListBox;
        }
    }

}
