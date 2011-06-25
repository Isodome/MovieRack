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
using WinMovieRack.Model;
using WinMovieRack.Controller;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for ListViewMovieInfo.xaml
    /// </summary>
    public partial class ListViewMovieInfo : UserControl
    {
        GUIMovie selectedMovie;
        ListViewMovieController controller;

        public ListViewMovieInfo(ListViewMovieController controller)
        {
            InitializeComponent();
            this.controller=  controller;
        }

        public void setMovieDetails(GUIMovie selectedMovie)
        {
            this.selectedMovie = selectedMovie;
            yearBox.Text = selectedMovie.year;
            movieTitleBox.Text = selectedMovie.title;
            plot.Text = selectedMovie.plot;
            originalTitle.Text = selectedMovie.originalTitle;
            imdbRating.Text = selectedMovie.imdbRating;
            runtime.Content = selectedMovie.runtime;
            imdbRatingDetails.Content = selectedMovie.imdbRating;
            top250.Content = selectedMovie.imdbTop250;
            seenCount.Content = selectedMovie.seenCount;
            lastSeen.Content = selectedMovie.lastSeen;
            metaReview.Content = selectedMovie.metacriticsReviewRating;
            metaUser.Content = selectedMovie.metacriticsUsersRating;
            tomatoe.Content = selectedMovie.tomatometer;
            tomatoeUser.Content = selectedMovie.rottenTomatoesAudience;
            otherWins.Content = selectedMovie.otherWins;
            otherNom.Content = selectedMovie.otherNominations;
            weeksInCine.Content = selectedMovie.weeksInCinema;
            budget.Content = selectedMovie.budget;
            boxWorld.Content = selectedMovie.boxofficeWorldwide;
            boxAmerica.Content = selectedMovie.boxofficeAmerica;
            boxForeign.Content = selectedMovie.boxofficeForeign;
            firstWeekend.Content = selectedMovie.boxofficeFirstWeekend;
            rankFirstWeekend.Content = selectedMovie.rangFirstWeekend;
            string languageString = generateString(controller.loadLanguageList(selectedMovie.dbId));
            if (languageString.Equals(""))
            {
                language.Text = "No Languages";
            }
            else
            {
                language.Text = languageString;
            }
            string genreString = generateString(controller.loadGenreList(selectedMovie.dbId));
            if (genreString.Equals(""))
            {
                genres.Text = "No Genres";
            }
            else
            {
                genres.Text = genreString;
            }
            controller.loadStarsListToMovie(selectedMovie.dbId, selectedMovie.yearInt);
            controller.loadProductionListToMovie(selectedMovie.dbId, selectedMovie.yearInt);

            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(selectedMovie.dbId, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterInfo.Source = posterBitmap;
        }

        private String generateString(List<String> listBoxMovies)
        {
            String generatedString = "";
            for (int i = 0; i < listBoxMovies.Count; i++)
            {
                generatedString += listBoxMovies.ElementAt(i);
                if (i < listBoxMovies.Count - 1)
                {
                    generatedString += ", ";
                }
            }
            return generatedString;
        }

        private void castExpander_Expanded(object sender, RoutedEventArgs e)
        {
            controller.loadActorListToMovie(selectedMovie.dbId, selectedMovie.yearInt);
        }

        private void posterInfo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BigPicture bigPicture = new WinMovieRack.GUI.BigPicture();
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(selectedMovie.dbId, PosterSize.FULL));
            posterBitmap.EndInit();
            bigPicture.bigPicture.Source = posterBitmap;
            Point origin = new Point(0, 0);
            Point screenOrigin = posterInfo.PointToScreen(origin);
            bigPicture.setOrigin(posterInfo.Source.Height, posterInfo.Source.Width, screenOrigin.X, screenOrigin.Y);
            bigPicture.ShowDialog();
        }


        private void starsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)starsListBox.SelectedItem;
            controller.personClicked(selectedItem);
        }

        private void productionListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)productionListBox.SelectedItem;
            controller.personClicked(selectedItem);
        }

        private void castListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)castListBox.SelectedItem;
            controller.personClicked(selectedItem);
        }

    }
}
