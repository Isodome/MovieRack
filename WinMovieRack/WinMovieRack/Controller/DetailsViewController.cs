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
            foreach (MRListData movie in completeMovieListData)
            {
                view.completeMovieListBox.Items.Add(new MovieRackListBoxItem(movie, true));
            }
        }

        public void loadActorListToMovie(int itemID, int year)
        {
            List<MRListData> actorListToMovie = db.getPersonListToMovie(itemID, year);
            completeActorList = new List<MovieRackListBoxItem>();
            foreach (MRListData actor in actorListToMovie)
            {
                MovieRackListBoxItem temp = new MovieRackListBoxItem(actor, false);
                completeActorList.Add(temp);
                view.completeCastListBox.Items.Add(temp);
            }
        }

        public void loadActorListPictures()
        {
            foreach (MovieRackListBoxItem actor in completeActorList)
            {
                actor.loadPicture();
            }
        }

        public void loadStarsList(int itemID, int year)
        {

            List<MRListData> mrListData = db.getStarsListToMovie(itemID, year);
            starsList = new List<MovieRackListBoxItem>();
            foreach (MRListData star in mrListData)
            {
                MovieRackListBoxItem temp = new MovieRackListBoxItem(star, false);
                starsList.Add(temp);
                view.addStarsListBoxItem(temp);
            }
        }

        public void loadProductionList(int itemID, int year)
        {
            List<MRListData> mrListData = db.getProductionListToMovie(itemID, year);
            productionList = new List<MovieRackListBoxItem>();
            foreach (MRListData production in mrListData)
            {
                MovieRackListBoxItem temp = new MovieRackListBoxItem(production, false);
                productionList.Add(temp);
                view.addProductionListBoxItem(temp);
            }
        }

        public void loadMovieListToPerson(int itemID)
        {
            view.completeMovieListToPerson.Items.Clear();
            List<MRListData> mrListData = db.getMovieListToPerson(itemID);
            foreach (MRListData movie in mrListData)
            {
                view.completeMovieListToPerson.Items.Add(new MovieRackListBoxItem(movie, true));
            }
        }


        public void loadStarsListPictures()
        {
            foreach (MovieRackListBoxItem star in starsList)
            {
                star.loadPicture();
            }
        }

        public void loadProductionListPictures()
        {
            foreach (MovieRackListBoxItem production in productionList)
            {
                production.loadPicture();
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
