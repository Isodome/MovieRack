﻿using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using WinMovieRack.Controller.Parser.imdbMovieParser;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using WinMovieRack.Controller.Parser.BoxOffice;
using System.Net;
using System.Threading;
namespace WinMovieRack.Controller
{
    public class Controller
    {

        private WinMovieRack.GUI.GUI gui;
        public SQLiteConnector db;
        public SQLiteConnectorTodo dbTodo;
        public static Controller controller;
        private App app;

        private ImdbBrowserController browserController;
        private TodoListController todoListController;
        private MainWindowController windowController;
        private DetailsViewController detailsViewController;
        private ActorsViewController actorsViewController;
        private ListViewController listViewController;

        public Controller(App app)
        {
            this.app = app;
            this.app.Exit += aboutToExit;
            initializeModel();
            initializeGUI();
            controller = this;
            ServicePointManager.DefaultConnectionLimit = 65000;

			if (System.Environment.MachineName.Equals("DOMI-PC")) {
				ThreadStart ts = () => {
					SerialBoxOfficeMovieParser b = new SerialBoxOfficeMovieParser("titanic");
					b.run();
					b.getResult().printToConsole();
					b = new SerialBoxOfficeMovieParser("abduction11");
					b.run();
					b.getResult().printToConsole();
					b = new SerialBoxOfficeMovieParser("inception");
					b.run();
					b.getResult().printToConsole();
				};
				(new Thread(ts)).Start();
			} else {
				Console.WriteLine("Nicht auf DOMI-PC sonder auf {0}, deswegen jetzt kein BO Parsen", System.Environment.MachineName);
			}
        }

        public void func(ConcThreadJobMaster sender)
        {
            ((ConcurrentImdbMovieParser)sender).movieData.printToConsole();
        }

        private void initializeGUI()
        {

            browserController = new ImdbBrowserController(this);
            IMDBBrowser browser = new IMDBBrowser(browserController);
            browserController.setBrowser(browser);

            todoListController = new TodoListController(this, dbTodo);
            TodoList todoList = new TodoList(todoListController);
            todoListController.setTodoList(todoList);

            windowController = new MainWindowController();
            MainWindow mw = new MainWindow(windowController);
            mw.Width = 1024;
            mw.Height = 600;
            windowController.setMainWindow(this, mw);

            detailsViewController = new DetailsViewController(this, db);
            DetailsView dv = new DetailsView(detailsViewController);
            detailsViewController.setDetailsView(dv);
            detailsViewController.loadCompleteMovieList();

            actorsViewController = new ActorsViewController(this, db);
            ActorsView av = new ActorsView(actorsViewController);
            actorsViewController.setActorsView(av);


            listViewController = new ListViewController(this, db);
            ListView lv = new ListView(listViewController);
            listViewController.setListView(lv);

            gui = new WinMovieRack.GUI.GUI(this, mw, browser, dv, av, lv, todoList);
        }

        private void initializeModel()
        {
            db = new SQLiteConnector();
            db.initDb();
            dbTodo = new SQLiteConnectorTodo();
            dbTodo.initDb();
            PictureHandler.initialize();
        }


        public void changeToView(View view)
        {
            //Inform specific controllers
            switch (view)
            {
                case View.ACTORS_VIEW:
                    actorsViewController.loadCompleteActorsList();
                    break;
                case View.DETAILS_VIEW:
                    detailsViewController.loadCompleteMovieList();
                    break;
                case View.IMDB_BROWSER:
                    browserController.activated();
                    break;
                case View.TODO_LIST:
                    browserController.activated();
                    break;
                case View.LIST_VIEW:
                    listViewController.loadlistView();
                    break;
            }
            gui.changeToView(view);
        }

        public void updateView(View view)
        {
            switch (view)
            {
                case View.ACTORS_VIEW:
                    break;
                case View.DETAILS_VIEW:
                    break;
                case View.IMDB_BROWSER:
                    break;
                case View.LIST_VIEW:
                    break;
                case View.TODO_LIST:
                    todoListController.update();
                    break;
            }
        }


        public void aboutToExit(object sender, ExitEventArgs e)
        {
            db.closeConnection();
            windowController.close();
            ThreadsMaster.getInstance().waitToFinish();
            Application.Current.Shutdown();
        }

        internal void setProgressIndicator(bool p)
        {
            windowController.setProgressIndicator(p);
        }
    }
}
