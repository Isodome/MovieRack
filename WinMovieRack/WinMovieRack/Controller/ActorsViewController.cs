using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;

namespace WinMovieRack.Controller
{
    public class ActorsViewController
    {
        private Controller controller;
        private SQLiteConnector db;
        private ActorsView view;
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> actorsList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> movieListToPerson = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> starsList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> productionList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
		private Action<MRListData> addToListFunction;
        private Dictionary<int, MRListBoxItem> listBoxItems = new Dictionary<int, MRListBoxItem>();
        private bool isFirstLoad = true;
        public ActorsViewController(Controller c, SQLiteConnector db)
        {
            this.db = db;
            this.controller = c;

        }

        public void setActorsView(ActorsView av)
        {
            this.view = av;
			view.listBoxActor.ItemsSource = actorsList;

			Dispatcher disp = Dispatcher.CurrentDispatcher;
			addToListFunction = (MRListData movie) => disp.BeginInvoke(DispatcherPriority.Background, (new Action(() => {
				Thread.Sleep(0);
				BitmapImage posterBitmap = new BitmapImage();
				posterBitmap.BeginInit();
				posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movie.dbItemID, PosterSize.LIST));
				posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
				posterBitmap.CacheOption = BitmapCacheOption.OnDemand;
				posterBitmap.EndInit();
				MRListBoxItem item = new MRListBoxItem(movie.dbItemID, movie.titleName, movie.yearAge.ToString(), movie.editableCharakter, posterBitmap);
				actorsList.Add(item);
				listBoxItems.Add(movie.dbItemID, item);
			})));

        }

        public void loadCompleteActorsList()
        {
            if (isFirstLoad) {
				isFirstLoad =  false;
                List<MRListData> completeMovieListData = db.getCompletePersonList(PersonEnum.gender, PersonEnum.Name);//Aus Config lesen
				var t = new Thread(() => db.completePersonListForEach(PersonEnum.gender, PersonEnum.Name, addToListFunction));
				t.Start();

            }
            
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

        public GUIPerson getGUIPerson(int itemID)
        {
            return db.getPersonInfo(itemID);
        }

        public GUIMovie getGUIMovie(int itemID)
        {
            return db.getMovieInfo(itemID);
        }

        public void deleteItem(int id)
        {
            MRListBoxItem itemToDelete;
            listBoxItems.TryGetValue(id, out itemToDelete);
            actorsList.Remove(itemToDelete);

        }

        public void addItem(int id, MRListBoxItem item)
        {
            actorsList.Add(item);
            listBoxItems.Add(id, item);
        }

        public void changeItem(int id, MRListBoxItem item)
        {
            MRListBoxItem itemToChange;
            listBoxItems.TryGetValue(id, out itemToChange);
            itemToChange = item;
        }

        public List<String> loadGenreList(int idMovies)
        {
            return db.getGenresToMovie(idMovies);
        }

        public List<String> loadLanguageList(int idMovies)
        {
            return db.getLanguageToMovie(idMovies);
        }
    }
}
