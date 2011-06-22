using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.GUI;

namespace WinMovieRack.Controller
{
    public class ListViewController
    {
        Controller controller;
        SQLiteConnector db;
        ListView view;

        public ListViewController(Controller c, SQLiteConnector db)
        {
            this.controller = c;
            this.db = db;
        }

        internal void setListView(GUI.ListView lv)
        {
            this.view = lv;
        }
    }
}
