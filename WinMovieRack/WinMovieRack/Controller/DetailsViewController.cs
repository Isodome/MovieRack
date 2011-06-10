using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
using WinMovieRack.Model;
namespace WinMovieRack.Controller
{

    public class DetailsViewController
    {

        private DetailsView view;
        private Controller controller;
        private SQLiteConnector db;
        private List<MRListData> mrListData;
        private List<MovieRackListBoxItem> movieRackListBoxItems;

        public DetailsViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
        }

        public void setDetailsView(DetailsView dv)
        {
            this.view = dv;
        }

        public void loadMovieList()
        {
            view.resetMovieList();
            createmovieRackListBoxItems();
            addMovieRackListBoxItem();

        }

        public void loadActorList(int itemID)
        {
            this.mrListData = db.getPersonListToMovie(itemID);
            this.movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                this.movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i),false));
            }
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.gernerateCastListBox(movieRackListBoxItems.ElementAt(i));
            }
        }

        public GUIPerson getGUIPerson(int itemID)
        {
            return db.getPersonInfo(itemID);
        }

        private void createmovieRackListBoxItems()
        {
            this.mrListData = db.getMovieList(MovieEnum.runtime);//Aus Config lesen
            this.movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                this.movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i),true));
            }
        }

        private void addMovieRackListBoxItem()
        {
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addMoviesListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
            view.sortListBox();
        }

        public GUIMovie getGUIMovie(int itemID)
        {
            return db.getMovieInfo(itemID);
        }
    }
}
