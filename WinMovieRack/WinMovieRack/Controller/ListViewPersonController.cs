using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.GUI;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace WinMovieRack.Controller
{
    public class ListViewPersonController
    {

        SQLiteConnector db;
        ListViewPersonInfo view;
        ListViewController controller;
        private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> movieListToPerson = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        public ListViewPersonController(SQLiteConnector db, ListViewController controller)
        {
            this.db = db;
            this.controller = controller;
        }
        public void setView(ListViewPersonInfo listViewPerson)
        {
            this.view = listViewPerson;
        }

        internal GUIPerson getPersonInfo(int personID)
        {
            return db.getPersonInfo(personID);
        }

        internal void loadMovieListToPerson(int personID)
        {
            movieListToPerson.Clear();
            List<MRListData> movieListPerson = db.getMovieListToPerson(personID);
            movieListPerson.ForEach(delegate(MRListData movie)
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(movie.dbItemID, PosterSize.LIST));
                posterBitmap.EndInit();
                movieListToPerson.Add(new MRListBoxItem(movie.dbItemID, movie.titleName, movie.yearAge.ToString(), movie.editableCharakter, posterBitmap));
            });
            view.movieListBox.ItemsSource = movieListToPerson;
        }

        internal void movieClicked(MRListBoxItem movie)
        {
            controller.changeToMovieView(db.getMovieInfo(movie.getId));
        }
    }
}
