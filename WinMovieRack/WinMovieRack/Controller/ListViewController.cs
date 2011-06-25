using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.GUI;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace WinMovieRack.Controller
{

   public class ListViewController {

        Controller controller;
        SQLiteConnector db;
        ListView view;
       ObservableCollection<GUIMovie> completeMovieList;
       private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> castList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
       private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> starsList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
       private ObservableCollection<WinMovieRack.GUI.MRListBoxItem> productionList = new ObservableCollection<WinMovieRack.GUI.MRListBoxItem>();
        public ListViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
        }

        internal void setListView(GUI.ListView lv)
        {
            this.view = lv;
        }

       public void loadlistView(){
          completeMovieList= db.getCompleteMovieInfo();
          view.movieList.ItemsSource = completeMovieList;
       }

       public void getGUIMovie(object sender)
       {
           GUIMovie movie = (GUIMovie)sender;
           
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

    }
}
