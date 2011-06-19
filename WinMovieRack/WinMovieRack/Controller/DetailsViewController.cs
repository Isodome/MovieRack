using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
namespace WinMovieRack.Controller
{

    public class DetailsViewController
    {

        private DetailsView view;
        private Controller controller;
        private SQLiteConnector db;
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> movieList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> castList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> starsList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> productionList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> movieListToPerson = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();

        public DetailsViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
        }

        public void setDetailsView(DetailsView dv)
        {
            this.view = dv;
        }

        public void loadCompleteMovieList()
        {
            movieList.Clear();
            List<MRListData> completeMovieListData = db.getCompleteMovieList(MovieEnum.runtime, MovieEnum.title);//Aus Config lesen
            completeMovieListData.ForEach(delegate(MRListData movie)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movie.dbItemID, PosterSize.LIST));
                posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                posterBitmap.CacheOption = BitmapCacheOption.OnDemand;
                posterBitmap.EndInit();
                movieList.Add(new MRListBoxItem(movie.dbItemID, movie.titleName, movie.yearAge.ToString(), movie.editableCharakter, posterBitmap));
            });
            view.listBoxMovies.ItemsSource = movieList;
        }

        public void loadActorListToMovie(int itemID, int year)
        {
            castList.Clear();
            List<MRListData> actorListToMovie = db.getPersonListToMovie(itemID, year);
            actorListToMovie.ForEach(delegate(MRListData actor)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(actor.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                castList.Add(new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap));
            });
            view.castListBox.ItemsSource = castList;

        }

        public void loadStarsListToMovie(int itemID, int year)
        {
            starsList.Clear();
            List<MRListData> starsListToMovie = db.getStarsListToMovie(itemID, year);
            starsListToMovie.ForEach(delegate(MRListData actor)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(actor.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                starsList.Add(new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap));
            });
            view.SummeryStarsListbox.ItemsSource = starsList;
        }

        public void loadProductionListToMovie(int itemID, int year)
        {
            productionList.Clear();
            List<MRListData> productionListToMovie = db.getProductionListToMovie(itemID, year);
            productionListToMovie.ForEach(delegate(MRListData actor)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(actor.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                productionList.Add(new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap));
            });
            view.SummeryProductionListbox.ItemsSource = productionList;
        }

        public void loadMovieListToPerson(int itemID)
        {
            movieListToPerson.Clear();
            List<MRListData> movieListPerson = db.getMovieListToPerson(itemID);
            movieListPerson.ForEach(delegate(MRListData movie)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movie.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                movieListToPerson.Add(new MRListBoxItem(movie.dbItemID, movie.titleName, movie.yearAge.ToString(), movie.editableCharakter, posterBitmap));
            });
            view.MovieListToPerson.ItemsSource = movieListToPerson;
        }


        public GUIPerson getGUIPerson(int itemID)
        {
            return db.getPersonInfo(itemID);
        }

        public GUIMovie getGUIMovie(int itemID)
        {
            return db.getMovieInfo(itemID);
        }

        public DataSet loadOtherAwards(int idMovie)
        {
            return db.getOtherAwardstoMovie(idMovie);
        }

        public DataSet loadOscarAwards(int idMovie)
        {
            return db.getOscarstoMovie(idMovie);
        }

        public List<String> loadGenreList(int idMovies)
        {
            return db.getGenresToMovie(idMovies);
        }

        public List<String> loadLanguageList(int idMovies)
        {
            return db.getLanguageToMovie(idMovies);
        }

        public void loadAlsoKnownAs(int idMovies)
        {
            List<String> alsoKnownas = db.getAlsoKnownToMovie(idMovies);
            foreach (String title in alsoKnownas)
            {
                view.alsoKownAs.Text += title;
                view.alsoKownAs.Text += "\n";
            }
        }
    }
}
