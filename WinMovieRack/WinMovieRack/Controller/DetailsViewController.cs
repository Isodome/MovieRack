using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WinMovieRack.GUI;
using WinMovieRack.Model;


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
        private ObservableCollection<WinMovieRack.GUI.SeenBoxItem> seenListToMovie;
        private Dictionary<int, MRListBoxItem> movieListItems = new Dictionary<int, MRListBoxItem>();
        private Dictionary<int, MRListBoxItem> castListItems = new Dictionary<int, MRListBoxItem>();
        private Dictionary<int, MRListBoxItem> productionListItems = new Dictionary<int, MRListBoxItem>();
        private bool isFirstLoad = true;
        private Action<MRListData> addToListFunction;

        public DetailsViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;





        }

        public void setDetailsView(DetailsView dv)
        {

            this.view = dv;
            view.listBoxMovies.ItemsSource = movieList;

            Dispatcher disp = Dispatcher.CurrentDispatcher;
            addToListFunction = (MRListData movie) => disp.BeginInvoke(DispatcherPriority.Background, (new Action(() =>
            {
                Thread.Sleep(0);
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movie.dbItemID, PosterSize.LIST));
                posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                posterBitmap.CacheOption = BitmapCacheOption.OnDemand;
                posterBitmap.EndInit();
                MRListBoxItem item = new MRListBoxItem(movie.dbItemID, movie.titleName, movie.yearAge.ToString(), movie.editableCharakter, posterBitmap);
                movieList.Add(item);
                movieListItems.Add(movie.dbItemID, item);
            })));

            /*
           
            var context = SynchronizationContext.Current;
			
            addToListFunction = (MRListData movie) => context.Send(delegate(object sender) {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movie.dbItemID, PosterSize.LIST));
                posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                posterBitmap.CacheOption = BitmapCacheOption.OnDemand;
                posterBitmap.EndInit();
                MRListBoxItem item = new MRListBoxItem(movie.dbItemID, movie.titleName, movie.yearAge.ToString(), movie.editableCharakter, posterBitmap);
                movieList.Add(item);
                movieListItems.Add(movie.dbItemID, item);

            }, null);
             */
        }


        public void loadCompleteMovieList()
        {
            if (isFirstLoad)
            {
                isFirstLoad = false;
                var t = new Thread(() => db.completeMovieListForEach(MovieEnum.runtime, MovieEnum.title, addToListFunction));
                t.Start();

            }
        }

        public void loadActorListToMovie(int itemID, int year)
        {
            castList.Clear();
            castListItems.Clear();
            List<MRListData> actorListToMovie = db.getPersonListToMovie(itemID, year);
            actorListToMovie.ForEach(delegate(MRListData actor)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(actor.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                MRListBoxItem item = new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap);
                castList.Add(item);
                castListItems.Add(actor.dbItemID, item);
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
            productionListItems.Clear();
            List<MRListData> productionListToMovie = db.getProductionListToMovie(itemID, year);
            productionListToMovie.ForEach(delegate(MRListData actor)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(actor.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                MRListBoxItem item = new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap);
                productionList.Add(item);
                productionListItems.Add(actor.dbItemID, item);
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

        public void loadSeenListToMovie(int idMovies)
        {
            seenListToMovie = new ObservableCollection<WinMovieRack.GUI.SeenBoxItem>();
            List<MRSeenData> seenList = db.getSeenList(idMovies);
            seenList.ForEach(delegate(MRSeenData seen)
           {
               seenListToMovie.Add(new SeenBoxItem(seen.date, seen.notes));
           });
            view.seenListBox.ItemsSource = seenListToMovie;
        }


        public void castListBoxSelectionChanged(int index)
        {
            if (view.personchange.SelectedIndex == 0)
            {
                view.castListBox.ItemsSource = castList;
                view.castListBox.SelectedIndex = 0;
                view.castBox.Header = "Cast";
            }
            else
            {
                view.castListBox.ItemsSource = productionList;
                view.castListBox.SelectedIndex = 0;
                view.castBox.Header = "Production";
            }
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

        public void deleteItem(int id)
        {
            MRListBoxItem itemToDelete;
            movieListItems.TryGetValue(id, out itemToDelete);
            movieList.Remove(itemToDelete);

        }

        public void addItem(int id, MRListBoxItem item)
        {
            movieList.Add(item);
            movieListItems.Add(id, item);
        }

        public void changeItem(int id, MRListBoxItem item)
        {
            MRListBoxItem itemToChange;
            movieListItems.TryGetValue(id, out itemToChange);
            itemToChange = item;
        }

        public MRListBoxItem getCastItem(int id)
        {
            MRListBoxItem itemToChange;
            castListItems.TryGetValue(id, out itemToChange);
            return itemToChange;
        }

        public MRListBoxItem getProductionItem(int id)
        {
            MRListBoxItem itemToChange;
            productionListItems.TryGetValue(id, out itemToChange);
            return itemToChange;
        }

        internal void setSeenDate(DateTime selectedDate, int id, string notes)
        {
            db.updateSeenToMovie(id, selectedDate, notes);
            view.seen.Content = (int)view.seen.Content + 1;
            view.loadSummerytab();
        }

        internal int getSeenCount(int idMovies)
        {
            return db.getSeenCount(idMovies);
        }
    }
}
