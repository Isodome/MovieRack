using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;

namespace WinMovieRack.GUI {
    class TodoListBoxItem {
        public TodoListData todo;
        public TodoListBoxItem(TodoListData todo)
        {
            this.todo = todo;
        }

        public string labelOne
        {
            get { return todo.title; }
            set { todo.title = value; }
        }
        public string labelTwo
        {
            get { return todo.description; }
            set { todo.description = value; }
        }

        public int getId
        {
            get { return todo.dbIdTodo; }
        }

    }
}
