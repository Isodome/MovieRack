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
        private List<MovieRackListBoxItem> completeActorList;
        private List<MovieRackListBoxItem> starsList;
        private List<MovieRackListBoxItem> productionList;

        public DetailsViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
        }

        public void setDetailsView(DetailsView dv)
        {
            this.view = dv;
        }

        public void loadCompleteMovieList()
        {
            view.completeMovieListBox.Items.Clear();
            List<MRListData> completeMovieListData = db.getCompleteMovieList(MovieEnum.runtime, MovieEnum.title);//Aus Config lesen
            List<MovieRackListBoxItem> movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < completeMovieListData.Count; i++)
            {
                view.completeMovieListBox.Items.Add(new MovieRackListBoxItem(completeMovieListData.ElementAt(i), true));
            }
        }

        public void loadActorList(int itemID)
        {
            List<MRListData> mrListData = db.getPersonListToMovie(itemID);
            completeActorList = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                completeActorList.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), false));
            }
            for (int i = 0; i < mrListData.Count; i++)
            {
                view.completeCastListBox.Items.Add(completeActorList.ElementAt(i));
            }
        }

        public void loadActorListPictures()
        {
            for (int i = 0; i < completeActorList.Count; i++)
            {
                completeActorList.ElementAt(i).loadPicture();
            }
        }

        public void loadStarsList(int itemID)
        {

            List<MRListData> mrListData = db.getStarsListToMovie(itemID);
            starsList = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                starsList.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), false));
            }
            for (int i = 0; i < starsList.Count; i++)
            {
                view.addStarsListBoxItem(starsList.ElementAt(i));
            }
        }

        public void loadProductionList(int itemID)
        {
            List<MRListData> mrListData = db.getProductionListToMovie(itemID);
            productionList = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                productionList.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), false));
            }
            for (int i = 0; i < productionList.Count; i++)
            {
                view.addProductionListBoxItem(productionList.ElementAt(i));
            }
        }

        public void loadMovieListToPerson(int itemID)
        {
            view.completeMovieListToPerson.Items.Clear();
            List<MRListData> mrListData = db.getMovieListToPerson(itemID);
            for (int i = 0; i < mrListData.Count; i++)
            {
                view.completeMovieListToPerson.Items.Add(new MovieRackListBoxItem(mrListData.ElementAt(i), true));
            }
        }


        public void loadStarsListPictures()
        {
            for (int i = 0; i < starsList.Count; i++)
            {
                starsList.ElementAt(i).loadPicture();
            }
        }

        public void loadProductionListPictures()
        {
            for (int i = 0; i < productionList.Count; i++)
            {
                productionList.ElementAt(i).loadPicture();
            }
        }

        public GUIPerson getGUIPerson(int itemID)
        {
            return db.getPersonInfo(itemID);
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
