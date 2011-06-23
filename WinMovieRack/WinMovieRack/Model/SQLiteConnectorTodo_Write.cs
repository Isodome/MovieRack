using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;

namespace WinMovieRack.Model {
    public partial class SQLiteConnectorTodo {

        private const string commandInsertTodo = "INSERT INTO TodoList (todoType, parameter, title, description) VALUES(@todoType, @parameter, @title, @description)";

        public int insertTodo(TodoListData todo) {
            SQLiteCommand command = new SQLiteCommand(connection);
            var param = new SQLiteParameter();

            command.CommandText = commandInsertTodo;
           

            param = new SQLiteParameter("@todoType") { Value = todo.todoType};
            command.Parameters.Add(param);
            param = new SQLiteParameter("@parameter") { Value = todo.parameter };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@title") { Value = todo.title };
            command.Parameters.Add(param);
            param = new SQLiteParameter("@description") { Value = todo.description };
            command.Parameters.Add(param);

            return executeCommandAndReturnID(command);
        }

        public void insertTodoWithActionOnTodo(TodoListData todo, Action<TodoListData> action) {
            todo.dbIdTodo = insertTodo(todo);
            action(todo);
        }
    }
}
