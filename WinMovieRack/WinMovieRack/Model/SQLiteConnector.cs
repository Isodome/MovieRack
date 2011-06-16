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

        public void initDb()
        {
            string dataSource = "MovieRack.db";
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + dataSource;
            connection.Open();
            this.checkTables();

			imdbPersonIds = new Dictionary<uint, int>();

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "select imdbID, idPerson FROM Person";
            SQLiteDataReader reader = executeReaderThreadSafe(command);
            while (reader.Read())
            {
                this.imdbPersonIds.Add(uint.Parse(reader["imdbID"].ToString()), int.Parse(reader["idPerson"].ToString()));
            }

            createFolders();
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


            // Erstellen der Tabelle, sofern diese noch nicht existiert.
            command.CommandText = script;
            executeCommandThreadSafe(command);

            // Einfügen eines Test-Datensatzes.
            command.CommandText = "INSERT INTO Movies (idMovies, year, title, plot, runtime, posterPath) VALUES(NULL, 1970, 'James Bond xx', 'laaangweilig', 120, 'Z:\\MovieRack\\WinMovieRack\\WinMovieRack\\Images\\#9.jpg')";
            //executeCommandThreadSafe(command);

            command.CommandText = "SELECT Plot, Title FROM Movies WHERE Year = 1970";

            SQLiteDataReader reader = executeReaderThreadSafe(command);

            while (reader.Read())
            {
                Console.WriteLine("Dies ist der {0}. eingefügte Datensatz mit dem Wert: \"{1}\"", reader[0].ToString(), reader[1].ToString());
            }
            // Freigabe der Ressourcen.
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
