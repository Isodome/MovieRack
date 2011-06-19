using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace WinMovieRack.Controller
{
    public class ActorsViewController
    {
        private Controller controller;
        private SQLiteConnector db;
        private ActorsView view;
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> actorsList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> movieListToPerson = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();

        public ActorsViewController(Controller c, SQLiteConnector db)
        {
            this.db = db;
            this.controller = c;

        }

        public void setActorsView(ActorsView av)
        {
            this.view = av;
        }

        public void loadCompleteActorsList()
        {
            actorsList.Clear();
            List<MRListData> completeMovieListData = db.getCompletePersonList(PersonEnum.gender, PersonEnum.Name);//Aus Config lesen
            completeMovieListData.ForEach(delegate(MRListData actor)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getPersonPortraitPath(actor.dbItemID, PosterSize.LIST));
                posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                posterBitmap.CacheOption = BitmapCacheOption.OnDemand;
                posterBitmap.EndInit();
                actorsList.Add(new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap));
            });
            view.listBoxActor.ItemsSource = actorsList;
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
    }
}
