using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
using WinMovieRack.Model;

namespace WinMovieRack.Controller
{
    public class ActorsViewController
    {
        private Controller controller;
        private SQLiteConnector db;
        private List<MRListData> mrListData;
        private List<MovieRackListBoxItem> movieRackListBoxItems;
        private ActorsView view;

        public ActorsViewController(Controller c, SQLiteConnector db)
        {
            this.db=db;
            this.controller = c;

        }
        public void setActorsView(ActorsView av)
        {
            this.view = av;
        }

        public void loadList()
        {
            
            createmovieRackListBoxItems();
            addMovieRackListBoxItem();
        }

        private void createmovieRackListBoxItems()
        {
            this.mrListData = db.getPersonList(PersonEnum.OscarWins);//Aus Config lesen
            this.movieRackListBoxItems = new List<MovieRackListBoxItem>();
            for (int i = 0; i < mrListData.Count; i++)
            {
                this.movieRackListBoxItems.Add(new MovieRackListBoxItem(mrListData.ElementAt(i),false));
            }
        }

        private void addMovieRackListBoxItem()
        {
            for (int i = 0; i < movieRackListBoxItems.Count; i++)
            {
                view.addActorListBoxItem(movieRackListBoxItems.ElementAt(i));
            }
        }
        public GUIPerson getGUIPerson(int itemID){
            return db.getPersonInfo(itemID);
        }
    }
}
