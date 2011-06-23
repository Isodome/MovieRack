using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model.Enums;

namespace WinMovieRack.Model {
    class TodoListData {
        public int dbIdTodo;
        public TodoType todoType;
        public string parameter;
        public string title;
        public string description;

        public TodoListData(TodoType todoType, string parameter, string title, string description) {
            this.todoType = todoType;
            this.parameter = parameter;
            this.title = title;
            this.description = description;
        }

        public TodoListData(int dbIdTodo, TodoType todoType, string parameter, string title, string description)
        {
            this.dbIdTodo = dbIdTodo;
            this.todoType = todoType;
            this.parameter = parameter;
            this.title = title;
            this.description = description;
        }

    }
}
