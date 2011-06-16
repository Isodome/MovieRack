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


        public DetailsView(DetailsViewController dvc)
        {
            InitializeComponent();
            this.controller = dvc;
            TextBlock plot1 = plot;
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

        public void addMoviesListBoxItem(MovieRackListBoxItem item)
        {
            listBoxMovies.Items.Add(item);
        }

        public void addCastListBoxItem(MovieRackListBoxItem item)
        {
            castListBox.Items.Add(item);
        }

        public void addMovieListToPersonItem(MovieRackListBoxItem item)
        {
            MovieListToPerson.Items.Add(item);
        }

        private void getMovieDetails(int itemID)
        {
            this.movieDetails = controller.getGUIMovie(itemID);
        }

        public void resetMovieList()
        {
            listBoxMovies.Items.Clear();
        }

        public void resetActorList()
        {
            castListBox.Items.Clear();
        }

        public void resetMovieListToPerson()
        {
            MovieListToPerson.Items.Clear();
        }

        public void resetSummeryStarsList()
        {
            SummeryStarsListbox.Items.Clear();
        }

        public void resetSummeryProductionList()
        {
            SummeryProductionListbox.Items.Clear();
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
            if (movieDetails.imdbTop250 > 0)
            {
                top250.Content = movieDetails.imdbTop250;
            }
            else
            {
                top250.Content = "N/A";
            }
            plot.Text = movieDetails.plot;
            genres.Content = genreString();
            language.Content = languageString();


            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movieDetails.dbId, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterTitle.Source = posterBitmap;
            runtime.Content = movieDetails.runtime;
         //   controller.loadProductionList(movieDetails.dbId);
          //  controller.loadStarsList(movieDetails.dbId);
            loadCastPictures(controller.loadProductionList(movieDetails.dbId));
            loadCastPictures(controller.loadStarsList(movieDetails.dbId));
        }
        private String genreString()
        {
            String genre = "";
            List<String> genreString = controller.loadGenreList(movieDetails.dbId);
            for (int i = 0; i < genreString.Count; i++)
            {
                genre += genreString.ElementAt(i);
                if (i != genreString.Count - 1)
                {
                    genre += " | ";
                }
            }
            return genre;

        }

        private String languageString()
        {
            String language = "";
            List<String> languageString = controller.loadLanguageList(movieDetails.dbId);
            for (int i = 0; i < languageString.Count; i++)
            {
                language += languageString.ElementAt(i);
                if (i != languageString.Count - 1)
                {
                    language += " , ";
                }
            }
            return language;
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
                getMovieDetails(selectetMovieItem.itemID);
                setMovieDetails(movieDetails);
                actorList = controller.loadActorList(selectetMovieItem.itemID);
                if (detailsViewTab.SelectedIndex == 2)
                {
                    loadCastPictures(actorList);
                }
                otherAwardsDataSet = null;
                oscarAwardsDataSet = null;
            }
        }

        private void detailsViewTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (detailsViewTab.SelectedIndex == 2)
            {
                //controller.loadActorList(selectetMovieItem.itemID); StackOverFlow, warum auch immer
                if (actorList != null)
                {
                    loadCastPictures(actorList);
                }
            }
            else if (detailsViewTab.SelectedIndex == 4)
            {
                if (otherAwardsDataSet == null)
                {
                    loadOtherAwards();
                }
                if (oscarAwardsDataSet == null)
                {
                    loadOscarAwards();
                }
            }
        }

        private void loadCastPictures(List<MovieRackListBoxItem> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list.ElementAt(i).loadPicture();
            }
        }

        private void loadOtherAwards()
        {
            otherAwardsDataSet = controller.loadOtherAwards(movieDetails.dbId);
            awardsGrid.DataContext = otherAwardsDataSet;
        }

        private void loadOscarAwards()
        {
            oscarAwardsDataSet = controller.loadOscarAwards(movieDetails.dbId);
            oscarGrid.DataContext = oscarAwardsDataSet;
        }

        //Muss noch besser gemacht werden, da bi vielen Filmen zu langsam, bzw. wenn Film in der Datenbank, aber nicht in der Liste Funktioniert es nicht
        private void MovieListToPerson_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MovieRackListBoxItem selectedItem = (MovieRackListBoxItem)MovieListToPerson.SelectedItem;
            getMovieDetails(selectedItem.itemID);
            setMovieDetails(movieDetails);
            actorList = controller.loadActorList(selectedItem.itemID);
            loadCastPictures(actorList);
            listBoxMovies.UnselectAll();
        }
    }

}
