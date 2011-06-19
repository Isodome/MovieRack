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
using WinMovieRack.Model;

namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for ActorsView.xaml
    /// </summary>
    public partial class ActorsView : UserControl
    {
        ActorsViewController controller;
        GUIPerson personDetails;
        private BigPicture bigPicture;
        private GUIMovie movieDetails;
        public ActorsView(ActorsViewController avc)
        {
            InitializeComponent();
            this.controller = avc;
        }

        private void listBoxActor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MRListBoxItem selectetItem = (MRListBoxItem)listBoxActor.SelectedItem;
            if (selectetItem != null)
            {
                this.personDetails = controller.getGUIPerson(selectetItem.getId);
                setPersonDetails(personDetails);
                controller.loadMovieListToPerson(selectetItem.getId);
            }
            MovieListToPerson.SelectedIndex = 0;
        }

        private void setPersonDetails(GUIPerson personDetails)
        {
            name.Content = personDetails.Name;
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
            posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            actorPoster.Source = posterBitmap;
        }

        private void setMovieDetails()
        {
            title.Text = movieDetails.title;
            year.Content = movieDetails.year;
            originalTitle.Content = movieDetails.originalTitle;
            imdbRating.Content = movieDetails.imdbRating;
            ownRating.Content = movieDetails.personalRating;
            runtime.Content = movieDetails.runtime;
            lastSeen.Content = movieDetails.lastSeen;
            // cinemaDate.Content = movieDetails.
            lastSeen.Content = movieDetails.lastSeen;
            //mpaa.Content = movieDetails.
            seen.Content = movieDetails.seenCount;
            //awards.Content = movieDetails.
            budget.Content = movieDetails.budget;
            boxxoffice.Content = movieDetails.boxofficeWorldwide;
            string genreString = generateString(controller.loadGenreList(movieDetails.dbId));
            if (genreString.Equals(""))
            {
                genres.Content = "No Genres";
            }
            else
            {
                genres.Content = genreString;
            }

            string languageString = generateString(controller.loadLanguageList(movieDetails.dbId));
            if (languageString.Equals(""))
            {
                language.Content = "No Languages";
            }
            else
            {
                language.Content = languageString;
            }
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

        private void loadMovieDetails(int movieID)
        {
            this.movieDetails = controller.getGUIMovie(movieID);
            setMovieDetails();
            controller.loadProductionListToMovie(movieID, movieDetails.yearInt);
            controller.loadStarsListToMovie(movieDetails.dbId, movieDetails.yearInt);
        }




        private void actorPoster_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bigPicture = new BigPicture();
            BitmapImage posterBitmap = new BitmapImage();
            posterBitmap.BeginInit();
            posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(personDetails.dbID, PosterSize.FULL));
            posterBitmap.EndInit();
            bigPicture.bigPicture.Source = posterBitmap;
            Point origin = new Point(0, 0);
            Point screenOrigin = actorPoster.PointToScreen(origin);
            bigPicture.setOrigin(actorPoster.Source.Height, actorPoster.Source.Width, screenOrigin.X, screenOrigin.Y);
            bigPicture.fadeIn();
        }

        private void MovieListToPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MRListBoxItem selectedMovieItem = (MRListBoxItem)MovieListToPerson.SelectedItem;
            if (selectedMovieItem != null)
            {
                loadMovieDetails(selectedMovieItem.getId);
            }
        }
    }
}
