using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using WinMovieRack.GUI;
using WinMovieRack.Model;
using WinMovieRack.Model.Enums;
using WinMovieRack.Controller.Moviefillout;


namespace WinMovieRack.Controller {
    public class TodoListController {
        private TodoList todoView;
        private Controller controller;
        private bool firstLoad = true;
        private SQLiteConnectorTodo dbTodo;

        private ObservableCollection<WinMovieRack.GUI.TodoListBoxItem> todoList = new ObservableCollection<WinMovieRack.GUI.TodoListBoxItem>();
        private Dictionary<int, TodoListBoxItem> todoListItems = new Dictionary<int, TodoListBoxItem>();

        public static Action<TodoListData> addToListFunction;

        private Dispatcher disp;

        public TodoListController(Controller c, SQLiteConnectorTodo dbTodo) {
            this.controller = c;
            this.dbTodo = dbTodo;
        }


        public void setTodoList(TodoList todoView) {
            this.todoView = todoView;
            todoView.todoListBox.ItemsSource = todoList;

            disp = Dispatcher.CurrentDispatcher;
            addToListFunction = (TodoListData todo) => disp.BeginInvoke(DispatcherPriority.Background, (new Action(() => {
                //System.Console.WriteLine(todo.description);
                TodoListBoxItem boxItem = new TodoListBoxItem(todo);
                todoList.Add(boxItem);
                todoListItems.Add(boxItem.getId, boxItem);
            })));

            updateTodoList();
        }

        public void doTodo(TodoListData todo) {
            switch (todo.todoType) {
                case TodoType.INSERT_MOVIE_BY_IMDB_ID:
                    startInsertMovieByImdbId(todo.parameter);
                    break;
            }
        }

        private void startInsertMovieByImdbId(string imdbId) {
            uint id = uint.Parse(imdbId);
            MovieFillOut f = new MovieFillOut(id);
            f.startFillout();
        }

        public void activated() {

            if (firstLoad) {
                firstLoad = false;
            }

            // updateTodoList();
        }

        private void updateTodoList() {
            /*disp.BeginInvoke(DispatcherPriority.Background, (new Action(() => {
                todoList.Clear();
                todoListItems.Clear();
            })));*/
           
            var t = new Thread(() => dbTodo.doActionOnCompleteTodoList(addToListFunction));
            t.Start();
        }

        public void update() {
            updateTodoList();
        }
    }
}
