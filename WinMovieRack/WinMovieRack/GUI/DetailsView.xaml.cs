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
using System.Diagnostics;
using System.Collections.ObjectModel;

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

        public DetailsView(DetailsViewController dvc)
        {
            InitializeComponent();
            this.controller = dvc;
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

        private void setMovieDetails()
        {
            yearLabel.Content = movieDetails.year;
            movieTitleLabel.Text = movieDetails.title;
            orgialTitleLabel.Content = movieDetails.originalTitle;
            imdbRating.Content = movieDetails.imdbRating;
            imdbVotes.Content = movieDetails.imdbRatingVotes;
            top250.Content = movieDetails.imdbTop250;
            ownRating.Content = movieDetails.personalRating;
            string genreString = generateString(controller.loadGenreList(movieDetails.dbId));
            if (genreString.Equals(""))
            {
                genres.Content = "No Genres";
            }
            else
            {
                genres.Content = genreString;
            }

            metascore.Content = movieDetails.metacriticsReviewRating;
            MetacriticsMetascoreCriticsVotes.Content = movieDetails.metacriticsReviewVotes;
            metacriticsUserRating.Content = movieDetails.metacriticsUsersRating;
            MetacriticsMetascoreUsersVotes.Content = movieDetails.metacriticsUserVotes;
            rottentomatoesTomatometerRating.Content = movieDetails.tomatometer;
            RottentomatoesTomatoemeterVotes.Content = movieDetails.tomatometerVotes;
            rottentomatoesAudienceRating.Content = movieDetails.rottenTomatoesAudience;
            RottentomatoesAudienceVotes.Content = movieDetails.rottenTomatoesAudienceVotes;

            plot.Text = movieDetails.plot;
            runtime.Content = movieDetails.runtime;
            // cinemaDate.Content = movieDetails.
            lastSeen.Content = movieDetails.lastSeen;
            //mpaa.Content = movieDetails.
            seen.Content = movieDetails.seenCount;
            //awards.Content = movieDetails.
            budget.Content = movieDetails.budget;
            boxxoffice.Content = movieDetails.boxofficeWorldwide;
            string languageString = generateString(controller.loadLanguageList(movieDetails.dbId));
            if (languageString.Equals(""))
            {
                language.Content = "No Languages";
            }
            else
            {
                language.Content = languageString;
            }
            SummeryNotes.Content = movieDetails.notes;

            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movieDetails.dbId, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            posterTitle.Source = posterBitmap;
        }

        private void setPersonDetails(GUIPerson personDetails, string editableCharacter)
        {
            personName.Text = personDetails.Name;
            charakter.Text = editableCharacter;
            locationOfBirth.Content = personDetails.CityofBirth;
            oscars.Content = personDetails.OscarWins;
            lifetimeGross.Content = personDetails.lifetimeGross;
            averageBoxoffice.Content = personDetails.boxofficeAverage;
            birthday.Content = personDetails.Birthday;
            oscars.Content = personDetails.OscarWins;
            lifetimeGross.Content = personDetails.lifetimeGross;
            averageBoxoffice.Content = personDetails.boxofficeAverage;
            ageDetails.Content = personDetails.age;
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(personDetails.dbID, PosterSize.PREVIEW));
            posterBitmap.EndInit();
            actorPoster.Source = posterBitmap;
        }

        private void loadMovieDetails(int movieID)
        {
            this.movieDetails = controller.getGUIMovie(movieID);
            setMovieDetails();
            controller.loadProductionListToMovie(movieID, movieDetails.yearInt);
            controller.loadStarsListToMovie(movieDetails.dbId, movieDetails.yearInt);
            controller.loadActorListToMovie(movieDetails.dbId, movieDetails.yearInt);
            loadSummerytab();
        }

        private void loadAwards()
        {
            oscarGrid.DataContext = controller.loadOscarAwards(movieDetails.dbId);
            awardsGrid.DataContext = controller.loadOtherAwards(movieDetails.dbId);
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

        private void loadSummerytab()
        {
            if (movieDetails != null)
            {
                if (detailsViewTab.SelectedIndex == 0)
                {
                }
                else if (detailsViewTab.SelectedIndex == 1)
                {
                    controller.loadAlsoKnownAs(movieDetails.dbId);
                }
                else if (detailsViewTab.SelectedIndex == 2)
                {
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

        private void listBoxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MRListBoxItem selectedMovieItem = (MRListBoxItem)listBoxMovies.SelectedItem;
            if (selectedMovieItem != null)
            {
                loadMovieDetails(selectedMovieItem.getId);
            }
            castListBox.SelectedIndex = 0;
            personchange.SelectedIndex = 0;
        }

        private void castListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MRListBoxItem selectedCastItem = (MRListBoxItem)castListBox.SelectedItem;
            if (selectedCastItem != null)
            {
                GUIPerson personDetails = controller.getGUIPerson(selectedCastItem.getId);
                setPersonDetails(personDetails, selectedCastItem.labelThree);
                controller.loadMovieListToPerson(selectedCastItem.getId);
            }
        }

        private void detailsViewTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadSummerytab();
        }

        private void MovieListToPerson_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)MovieListToPerson.SelectedItem;
            loadMovieDetails(selectedItem.getId);
            listBoxMovies.UnselectAll();
        }

        private void actorPoster_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)castListBox.SelectedItem;
            if (selectedItem != null)
            {
                bigPicture = new BigPicture();
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(selectedItem.getId, PosterSize.FULL));
                posterBitmap.EndInit();
                bigPicture.bigPicture.Source = posterBitmap;
                Point origin = new Point(0, 0);
                Point screenOrigin = actorPoster.PointToScreen(origin);
                bigPicture.setOrigin(actorPoster.Source.Height, actorPoster.Source.Width, screenOrigin.X, screenOrigin.Y);
                bigPicture.fadeIn();
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

        private void imdbLink_Click(object sender, RoutedEventArgs e)
        {
            if (movieDetails != null)
            {
                Process.Start("http://www.imdb.com/title/tt" + movieDetails.imdbID);
            }
        }

        private void SummeryStarsListbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MRListBoxItem selectedItem = (MRListBoxItem)SummeryStarsListbox.SelectedItem;
            MRListBoxItem itemToSelect = controller.getitem(selectedItem.getId);

            detailsViewTab.SelectedIndex = 2;
            castListBox.SelectedItem = itemToSelect;

        }

        private void personchange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (controller != null)
            {
                controller.castListBoxSelectionChanged(personchange.SelectedIndex);
            }
        }
    }
}
