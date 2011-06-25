using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using System.Windows.Media.Imaging;
using WinMovieRack.GUI;
using System.Collections.ObjectModel;

namespace WinMovieRack.Controller
{
    public class ListViewMovieController
    {
        SQLiteConnector db;
        ListViewMovieInfo view;
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> castList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> starsList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> productionList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        ListViewController controller;
        public ListViewMovieController(SQLiteConnector db, ListViewController controller)
        {
            this.db = db;
            this.controller = controller;
        }

        public void setView(ListViewMovieInfo ListViewMovie)
        {
            this.view = ListViewMovie;
        }

        internal void personClicked(MRListBoxItem person)
        {
            controller.changeToPersonView(person.getId);
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
                MRListBoxItem item = new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap);
                castList.Add(item);
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
            view.starsListBox.ItemsSource = starsList;
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
                MRListBoxItem item = new MRListBoxItem(actor.dbItemID, actor.titleName, actor.yearAge.ToString(), actor.editableCharakter, posterBitmap);
                productionList.Add(item);
            });
            view.productionListBox.ItemsSource = productionList;
        }

        public List<String> loadGenreList(int idMovies)
        {
            return db.getGenresToMovie(idMovies);
        }

        public List<String> loadLanguageList(int idMovies)
        {
            return db.getLanguageToMovie(idMovies);
        }


        internal void updateSeenCount()
        {
            view.seenCountLabel.Content = int.Parse(view.seenCount.Content.ToString()) + 1;
        }
    }
}
