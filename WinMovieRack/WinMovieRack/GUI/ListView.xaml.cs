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
        GUIMovie selectedMovie;
        public ListView(ListViewController lvc)
        {
            InitializeComponent();
            this.controller = lvc;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Test");
        }

        private void movieList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            selectedMovie = (GUIMovie)movieList.SelectedItem;
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

        private void castExpander_Expanded(object sender, RoutedEventArgs e)
        {
            controller.loadActorListToMovie(selectedMovie.dbId, selectedMovie.yearInt);
        }
    }

}

