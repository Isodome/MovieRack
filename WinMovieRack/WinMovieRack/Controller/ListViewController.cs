using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.GUI;
using System.Collections.ObjectModel;

namespace WinMovieRack.Controller
{

   public class ListViewController {

        Controller controller;
        SQLiteConnector db;
        ListView view;
       ObservableCollection<GUIMovie> completeMovieList
           ;
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
    }
}
