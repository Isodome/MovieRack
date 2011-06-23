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
                int years = 0;
                if (reader.GetDateTime(2).Year != 1) {
                    DateTime now = DateTime.Today;
                    years = now.Year - reader.GetDateTime(2).Year;
                    if (reader.GetDateTime(2) > now.AddYears(-years)) years--;
                }
                todoList.Add(new TodoListData(reader.GetInt32(0), (TodoType) reader.GetInt32(1), reader["parameter"].ToString(), reader["title"].ToString(), reader["description"].ToString()));
            }
            command.Dispose();
            return todoList;
        }
    }
}
