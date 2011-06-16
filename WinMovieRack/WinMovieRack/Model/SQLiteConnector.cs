using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using System.Collections.Generic;
namespace WinMovieRack.Model
{
    public partial class SQLiteConnector
    {
        private object DBProtect = new object();
        private SQLiteConnection connection;
		private Dictionary<uint, int> imdbPersonIds;
		private Dictionary<uint, int> imdbMoviesIds;
		public static SQLiteConnector db;


        public void initDb()
        {
            string dataSource = "MovieRack.db";
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + dataSource;
            connection.Open();
            this.checkTables();

			imdbPersonIds = new Dictionary<uint, int>();
			imdbMoviesIds = new Dictionary<uint, int>();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "select imdbID, idPerson FROM Person";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                this.imdbPersonIds.Add(uint.Parse(reader["imdbID"].ToString()), int.Parse(reader["idPerson"].ToString()));
            }

			SQLiteCommand command2 = new SQLiteCommand(connection); 
			command2.CommandText = "select idMovies, imdbID FROM Movies";
			reader = executeReaderThreadSafe(command2);
			while (reader.Read()) {
				this.imdbMoviesIds.Add(uint.Parse(reader["imdbID"].ToString()), int.Parse(reader["idMovies"].ToString()));
			}

            createFolders();
			db = this;
        }

        private void createFolders()
        {
            Directory.CreateDirectory("img\\mov");
            Directory.CreateDirectory("img\\per");
        }

        public void checkTables()
        {
            FileInfo file = new FileInfo("createscript.sql");
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




        public bool testAndSetPerson(uint imdbID, out int idPerson)
        {
            bool contains;
            lock (this.imdbPersonIds)
            {
				contains = this.imdbPersonIds.TryGetValue(imdbID, out idPerson);
                if (!contains)
                {
                    SQLiteCommand command = new SQLiteCommand(connection);
                    command.CommandText = "insert into Person (imdbID) values (" + imdbID.ToString() + ")";
					idPerson = executeCommandAndReturnID(command);
					this.imdbPersonIds.Add(imdbID, idPerson);
                }
            }
            return contains;

        }

		public bool testAndSetMovies(uint imdbID, out int idMovies) {
			bool contains;
			lock (this.imdbMoviesIds) {
				contains = this.imdbMoviesIds.TryGetValue(imdbID, out idMovies);
				if (!contains) {
					SQLiteCommand command = new SQLiteCommand(connection);
					command.CommandText = "insert into Movies (imdbID) values (" + imdbID.ToString() + ")";
					idMovies = executeCommandAndReturnID(command);
					this.imdbMoviesIds.Add(imdbID, idMovies);
				}
			}
			return contains;
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



        private int getIdPersonByImdbId(uint personImdbId)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idPerson FROM Person WHERE imdbID=" + personImdbId.ToString();
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            if (reader.Read())
            {
                return int.Parse(reader[0].ToString());
            }
            return -1;
        }

        private int getIdMoviesByImdbId(uint movieImdbId)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT idMovies FROM Movies WHERE imdbID = " + movieImdbId.ToString();
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            if (reader.Read())
            {
                return int.Parse(reader[0].ToString());
            }
            return -1;
        }

		private int getIDCountryByCountryName(string country) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = String.Format("SELECT idCountry FROM Country WHERE Country='{0}'", country);
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			if (reader.Read()) {
				return reader.GetInt32(0);
			}
			return -1;
		}

		private int getIDLanguageByLanguage(string lang) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = String.Format("SELECT idLanguage FROM Language WHERE Language='{0}'", lang);
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			if (reader.Read()) {
				return reader.GetInt32(0);
			}
			return -1;
		}

		private int getIDGenreByGenre(string genre) {
			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = String.Format("SELECT idGenre FROM Genre WHERE Genre='{0}'", genre);
			SQLiteDataReader reader = executeReaderThreadSafe(command);
			if (reader.Read()) {
				return reader.GetInt32(0);
			}
			return -1;
		}

    }
}
