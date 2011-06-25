using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.GUI;

namespace WinMovieRack.Controller
{
    public class ListViewPersonController
    {

        SQLiteConnector db;
        ListViewPersonInfo view;
        ListViewController controller;
        public ListViewPersonController(SQLiteConnector db, ListViewController controller)
        {
            this.db = db;
            this.controller = controller;
        }
        public void setView(ListViewPersonInfo listViewPerson)
        {
            this.view = listViewPerson;
        }

        internal GUIPerson getPersonInfo(int personID)
        {
            return db.getPersonInfo(personID);
        }
    }
}
