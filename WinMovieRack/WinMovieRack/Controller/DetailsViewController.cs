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
        SQLiteConnector db;
        List<MRListData> mrListData;
        List<MovieRackListBoxItem> movieRackListBoxItems;

        public DetailsViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
          //  createmovieRackListBoxItems();
           // addMovieRackListBoxItem();
        }

        public void setDetailsView(DetailsView dv)
        {
            this.view = dv;
        }

        private void createmovieRackListBoxItems()
        {
            this.mrListData = db.getMovieList(MovieEnum.Runtime);//Aus Config lesen
            this.movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                this.movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i)));
            }
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
