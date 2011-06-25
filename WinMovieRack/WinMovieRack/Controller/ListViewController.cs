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

    public class ListViewController : GUIController
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
        private bool isLoad = false;

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
            view.infoGrid.Children.Remove(view.noMovie);
            lvM.setMovieDetails(movie);
            view.infoGrid.Children.Remove(current);
            Grid.SetColumn(lvM, 0);
            Grid.SetRow(lvM, 0);
            Grid.SetRowSpan(lvM, 2);
            view.infoGrid.Children.Add(lvM);
            current = lvM;
        }

        public void changeToPersonView(int personID)
        {
            lvP.setPersonInfo(personID);
            view.infoGrid.Children.Remove(current);
            Grid.SetColumn(lvP, 0);
            Grid.SetRow(lvP, 0);
            Grid.SetRowSpan(lvP, 2);
            view.infoGrid.Children.Add(lvP);
            current = lvP;
        }

        internal void loadlistView()
        {
            if (!isLoad)
            {
                completeMovieList = db.getCompleteMovieInfo();
                view.movieList.ItemsSource = completeMovieList;
                isLoad = true;
            }
        }

        internal override void setSeenDate(DateTime selectedDate, int id, string notes)
        {
            db.updateSeenToMovie(id, selectedDate, notes);
            lvMController.updateSeenCount();
        }

        internal override void addList(string name)
        {
        }
    }
}
