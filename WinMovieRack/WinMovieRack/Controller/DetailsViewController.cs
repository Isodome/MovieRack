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

        public void loadList()
        {
            view.resetMovieList();
            createmovieRackListBoxItems();
            addMovieRackListBoxItem();
        }

        private void createmovieRackListBoxItems()
        {
            this.mrListData = db.getMovieList(MovieEnum.runtime);//Aus Config lesen
            this.movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                this.movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i)));
            }
            db.getMovieInfo(1);
        }

        private void addMovieRackListBoxItem()
        {
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addMoviesListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
        }
    }
}
