using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using WinMovieRack.Model.Enums;

namespace WinMovieRack.Model {
    public partial class SQLiteConnectorTodo {
        public List<TodoListData> getCompleteTodoList() {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<TodoListData> todoList = new List<TodoListData>();
            command.CommandText = "SELECT * FROM TodoList ORDER BY idTodo ASC;";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read()) {
                todoList.Add(new TodoListData(reader.GetInt32(0), (TodoType) reader.GetInt32(1), reader["parameter"].ToString(), reader["title"].ToString(), reader["description"].ToString()));
            }
            command.Dispose();
            return todoList;
        }

        public void doActionOnCompleteTodoList(Action<TodoListData> action) {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<TodoListData> todoList = new List<TodoListData>();
            command.CommandText = "SELECT * FROM TodoList ORDER BY idTodo ASC;";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read()) {
                action(new TodoListData(reader.GetInt32(0), (TodoType)reader.GetInt32(1), reader["parameter"].ToString(), reader["title"].ToString(), reader["description"].ToString()));
            }
            command.Dispose();
        }
    }
}
