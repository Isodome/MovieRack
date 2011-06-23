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


namespace WinMovieRack.Controller {
    public class TodoListController {
        private TodoList todoView;
        private Controller controller;
        private bool firstLoad = true;
        private SQLiteConnectorTodo dbTodo;

        private ObservableCollection<WinMovieRack.GUI.TodoListBoxItem> todoList = new ObservableCollection<WinMovieRack.GUI.TodoListBoxItem>();
        private Dictionary<int, TodoListBoxItem> todoListItems = new Dictionary<int, TodoListBoxItem>();

        public TodoListController(Controller c, SQLiteConnectorTodo dbTodo) {
            this.controller = c;
            this.dbTodo = dbTodo;
        }


        public void setTodoList(TodoList todoView) {
            this.todoView = todoView;
            todoView.todoListBox.ItemsSource = todoListItems;
            updateTodoList();
        }

        public void activated() {

            if (firstLoad) {
                firstLoad = false;
            }

            updateTodoList();
        }

        private void updateTodoList() {
            todoList = new ObservableCollection<WinMovieRack.GUI.TodoListBoxItem>();
            todoListItems = new Dictionary<int, TodoListBoxItem>();

            List<TodoListData> todoListData = dbTodo.getCompleteTodoList();
            foreach (TodoListData d in todoListData) {
                TodoListBoxItem boxItem = new TodoListBoxItem(d);
                todoList.Add(boxItem);
                todoListItems.Add(boxItem.getId, boxItem);
            }
        }

    }
}
