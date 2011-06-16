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
using System.Data;

namespace WinMovieRack
{
    /// <summary>
    /// Interaction logic for Movies.xaml
    /// </summary>
    public partial class DetailsView : System.Windows.Controls.UserControl
    {
        private UIElement current;
        private BigPicture bigPicture;
        private GUIMovie movieDetails;
        private DetailsViewController controller;
        private MovieRackListBoxItem selectetMovieItem;
        private List<MovieRackListBoxItem> actorList;
        DataSet otherAwardsDataSet;
        DataSet oscarAwardsDataSet;
        public System.Windows.Controls.ListBox completeMovieListBox;
        public System.Windows.Controls.ListBox completeCastListBox;
        public System.Windows.Controls.ListBox completeMovieListToPerson;
        public DetailsView(DetailsViewController dvc)
        {
            InitializeComponent();
            this.controller = dvc;
            completeMovieListBox = listBoxMovies;
            completeCastListBox = castListBox;
            completeMovieListToPerson = MovieListToPerson;
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

        public void addStarsListBoxItem(MovieRackListBoxItem item)
        {
            SummeryStarsListbox.Items.Add(item);
        }

        public void addProductionListBoxItem(MovieRackListBoxItem item)
        {
            SummeryProductionListbox.Items.Add(item);
        }

        private void setMovieDetails(GUIMovie movieDetails)
        {
            this.movieDetails = movieDetails;
            yearLabel.Content = "(" + movieDetails.year + ")";
            movieTitleLabel.Text = movieDetails.title;
            orgialTitleLabel.Content = movieDetails.originalTitle;
            imdbRating.Content = (double)movieDetails.imdbRating / 10.0 + "/10";
            imdbVotes.Content = "(" + movieDetails.imdbRatingVotes + " Votes)";
            top250.Content = movieDetails.imdbTop250;
            plot.Text = movieDetails.plot;
            genres.Content = generateString(controller.loadGenreList(movieDetails.dbId));
            language.Content = generateString(controller.loadLanguageList(movieDetails.dbId));
            runtime.Content = movieDetails.runtime;

            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movieDetails.dbId, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterTitle.Source = posterBitmap;
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

        private void deleteMovieDetails()
        {
            castListBox.Items.Clear();
            MovieListToPerson.Items.Clear();
            SummeryStarsListbox.Items.Clear();
            SummeryProductionListbox.Items.Clear();
            movieDetails = null;
            otherAwardsDataSet = null;
            oscarAwardsDataSet = null;
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(-1, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            actorPoster.Source = posterBitmap;
            personName.Text = null;

        }

        private void loadMovieDetails(int movieID)
        {
            this.movieDetails = controller.getGUIMovie(movieID);
            setMovieDetails(movieDetails);
            controller.loadActorList(movieID);
            controller.loadProductionList(movieID);
            controller.loadStarsList(movieID);
            controller.loadProductionListPictures();
            controller.loadStarsListPictures();
            if (detailsViewTab.SelectedIndex == 0)
            {
            }
            else if (detailsViewTab.SelectedIndex == 2)
            {
                controller.loadActorListPictures();
            }
            else if (detailsViewTab.SelectedIndex == 3)
            {
            }
            else if (detailsViewTab.SelectedIndex == 4)
            {
                loadAwards();
            }
            else if (detailsViewTab.SelectedIndex == 5)
            {
            }
        }

        private void loadAwards()
        {
            oscarAwardsDataSet = controller.loadOscarAwards(movieDetails.dbId);
            oscarGrid.DataContext = oscarAwardsDataSet;
            otherAwardsDataSet = controller.loadOtherAwards(movieDetails.dbId);
            awardsGrid.DataContext = otherAwardsDataSet;
        }

        private String generateString(List<String> listBoxMovies)
        {
            String generatetString = "";
            for (int i = 0; i < listBoxMovies.Count; i++)
            {
                generatetString += listBoxMovies.ElementAt(i);
                if (i < listBoxMovies.Count - 1)
                {
                    generatetString += ", ";
                }
            }
            return generatetString;
        }

        private void MovieListToPerson_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MovieRackListBoxItem selectedItem = (MovieRackListBoxItem)MovieListToPerson.SelectedItem;
            deleteMovieDetails();
            loadMovieDetails(selectedItem.itemID);
            listBoxMovies.UnselectAll();
        }

        private void castListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieRackListBoxItem selectetItem = (MovieRackListBoxItem)castListBox.SelectedItem;
            if (selectetItem != null)
            {
                GUIPerson personDetails = controller.getGUIPerson(selectetItem.itemID);
                setPersonDetails(personDetails);
                controller.loadMovieListToPerson(selectetItem.itemID);
            }
        }

        private void listBoxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectetMovieItem = (MovieRackListBoxItem)listBoxMovies.SelectedItem;
            if (selectetMovieItem != null)
            {
                deleteMovieDetails();
                loadMovieDetails(selectetMovieItem.itemID);
            }
            castListBox.SelectedIndex = 0;
        }


        private void actorPoster_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MovieRackListBoxItem selectetItem = (MovieRackListBoxItem)castListBox.SelectedItem;
            bigPicture = new BigPicture();
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(selectetItem.itemID, PosterSize.FULL));
            posterBitmap.EndInit();
            bigPicture.bigPicture.Source = posterBitmap;
            Point origin = new Point(0, 0);
            Point screenOrigin = actorPoster.PointToScreen(origin);
            bigPicture.setOrigin(actorPoster.Source.Height, actorPoster.Source.Width, screenOrigin.X, screenOrigin.Y);
            bigPicture.fadeIn();
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

        private void detailsViewTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (movieDetails != null)
            {
                if (detailsViewTab.SelectedIndex == 0)
                {
                }
                else if (detailsViewTab.SelectedIndex == 1)
                {
                }
                else if (detailsViewTab.SelectedIndex == 2)
                {
                    controller.loadActorListPictures();
                }
                else if (detailsViewTab.SelectedIndex == 3)
                {
                }
                else if (detailsViewTab.SelectedIndex == 4)
                {
                    loadAwards();
                }
                else if (detailsViewTab.SelectedIndex == 5)
                {
                }
            }
        }
    }
}
