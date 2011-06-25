using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.GUI;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace WinMovieRack.Controller
{

    public class ListViewController
    {
        private UIElement current;
        private Controller controller;
        private SQLiteConnector db;
        private WinMovieRack.GUI.ListView view;
        private ObservableCollection<GUIMovie> completeMovieList;
        private ListViewMovieInfo lvM;
        private ListViewPersonInfo lvP;
        private ListViewPersonController lvPController;
        private ListViewMovieController lvMController;


        public ListViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
            lvPController = new ListViewPersonController(db, this);
            lvMController = new ListViewMovieController(db, this);
            lvM = new ListViewMovieInfo(lvMController);
            lvP = new ListViewPersonInfo(lvPController);
            lvMController.setView(lvM);
            lvPController.setView(lvP);
        }

        internal void setListView(GUI.ListView lv)
        {
            this.view = lv;
        }

        public void changeToMovieView(GUIMovie movie)
        {
            lvM.setMovieDetails(movie);
            view.infoGrid.Children.Remove(current);
            Grid.SetColumn(lvM, 0);
            Grid.SetRow(lvM, 0);
            view.infoGrid.Children.Add(lvM);
            current = lvM;
        }

        public void changeToPersonView(int personID)
        {
            lvP.setPersonInfo(personID);
            view.infoGrid.Children.Remove(current);
            Grid.SetColumn(lvM, 0);
            Grid.SetRow(lvM, 0);
            view.infoGrid.Children.Add(lvP);
            current = lvP;
        }

        internal void loadlistView()
        {
            completeMovieList = db.getCompleteMovieInfo();
            view.movieList.ItemsSource = completeMovieList;
        }
    }
}
