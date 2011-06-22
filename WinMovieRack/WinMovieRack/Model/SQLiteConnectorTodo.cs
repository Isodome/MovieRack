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
    partial class SQLiteConnectorTodo {
        private object DBProtect = new object();
        private SQLiteConnection connection;
		public static SQLiteConnectorTodo db;


        public void initDb()
        {
            string dataSource = "MovieRackTodo.db";
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + dataSource;
            connection.Open();
            this.checkTables();
			db = this;
        }


        public void checkTables()
        {
            FileInfo file = new FileInfo("createscriptTodo.sql");
            string script = file.OpenText().ReadToEnd();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = script;
            executeCommandThreadSafe(command);
            command.Dispose();

        }
		public void beginTransaction() {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "BEGIN TRANSACTION";
		}
		public void endTransaction() {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "COMMIT";
		}

        public void closeConnection()
        {
            connection.Dispose();
            connection.Close();
        }

        private void executeCommandThreadSafe(SQLiteCommand command)
        {
            lock (DBProtect)
            {
                command.ExecuteNonQuery();
            }
        }

        private SQLiteDataReader executeReaderThreadSafe(SQLiteCommand command)
        {
            SQLiteDataReader r;
            lock (DBProtect)
            {
                r = command.ExecuteReader();
            }
            return r;
        }

		private int executeCommandAndReturnID(SQLiteCommand command) {
			string getIDCommand = "select last_insert_rowid()";
			SQLiteCommand idQuery = new SQLiteCommand(connection);
			idQuery.CommandText = getIDCommand;

			int id = -1;
			lock (DBProtect) {
				command.ExecuteNonQuery();
				id = int.Parse(idQuery.ExecuteScalar().ToString());
			}
			return id;
		}

    }
}
