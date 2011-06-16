using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using System.Data;
namespace WinMovieRack.Controller
{

    public class DetailsViewController
    {

        private DetailsView view;
        private Controller controller;
        private SQLiteConnector db;
        //  private List<MRListData> mrListData;
        //private List<MovieRackListBoxItem> movieRackListBoxItems;

        public DetailsViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
        }

        public void setDetailsView(DetailsView dv)
        {
            this.view = dv;
        }

        public List<MovieRackListBoxItem> loadActorList(int itemID)
        {
            view.resetActorList();
            List<MRListData> mrListData = db.getPersonListToMovie(itemID);
            List<MovieRackListBoxItem> movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), false));
            }
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addCastListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
            view.castListBox.SelectedIndex = 0;
            return movieRackListBoxItems;
        }

        public List<MovieRackListBoxItem> loadStarsList(int itemID)
        {
            view.resetSummeryStarsList();
            List<MRListData> mrListData = db.getStarsListToMovie(itemID);
            List<MovieRackListBoxItem> movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), false));
            }
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addStarsListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
            return movieRackListBoxItems;
        }

        public List<MovieRackListBoxItem> loadProductionList(int itemID)
        {
            view.resetSummeryProductionList();
            List<MRListData> mrListData = db.getProductionListToMovie(itemID);
            List<MovieRackListBoxItem> movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), false));
            }
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addProductionListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
            return movieRackListBoxItems;
        }

        public void loadMovieList()
        {
            view.resetMovieList();
            List<MRListData> mrListData = db.getCompleteMovieList(MovieEnum.runtime, MovieEnum.title);//Aus Config lesen
            List<MovieRackListBoxItem> movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), true));
            }
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addMoviesListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
        }

        public GUIPerson getGUIPerson(int itemID)
        {
            return db.getPersonInfo(itemID);
        }

        public void loadMovieListToPerson(int itemID)
        {
            view.resetMovieListToPerson();
            List<MRListData> mrListData = db.getMovieListToPerson(itemID);
            List<MovieRackListBoxItem> movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), true));
            }
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addMovieListToPersonItem(movieRackListBoxItems.ElementAt(i));
            }
        }

        public GUIMovie getGUIMovie(int itemID)
        {
            return db.getMovieInfo(itemID);
        }

        public DataSet loadOtherAwards(int idMovie)
        {
            return db.getOtherAwardstoMovie(idMovie);
        }


        public DataSet loadOscarAwards(int idMovie)
        {
            return db.getOscarstoMovie(idMovie);
        }

        public List<String> loadGenreList(int idMovies)
        {
            return db.getGenres(idMovies);
        }


        public List<String> loadLanguageList(int idMovies)
        {
            return db.getLanguageToMovie(idMovies);
        }
    }
}
