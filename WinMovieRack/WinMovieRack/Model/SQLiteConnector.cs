using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
namespace WinMovieRack.Model {
	public class SQLiteConnector {
		private SQLiteConnection connection;
		private HashSet<uint> imdbPersonIds;

		public void initDb() {
			string dataSource = "MovieRack.db";
			connection = new SQLiteConnection();
			connection.ConnectionString = "Data Source=" + dataSource;
			connection.Open();
			this.checkTables();

			imdbPersonIds = new HashSet<uint>();

			SQLiteCommand command = new SQLiteCommand(connection);
			command.CommandText = "select imdbID FROM Person";
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				this.imdbPersonIds.Add(uint.Parse(reader[0].ToString()));
			}

		}

		public void checkTables() {
			FileInfo file = new FileInfo("createscript.sql");
			string script = file.OpenText().ReadToEnd();

			SQLiteCommand command = new SQLiteCommand(connection);


			// Erstellen der Tabelle, sofern diese noch nicht existiert.
			command.CommandText = script;
			command.ExecuteNonQuery();
			
			// Einfügen eines Test-Datensatzes.
			command.CommandText = "INSERT INTO Movies (idMovies, year, title, plot, runtime, posterPath) VALUES(NULL, 1970, 'James Bond xx', 'laaangweilig', 120, 'Z:\\MovieRack\\WinMovieRack\\WinMovieRack\\Images\\#9.jpg')";
			command.ExecuteNonQuery();

			command.CommandText = "SELECT Plot, Title FROM Movies WHERE Year = 1970";

			SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read()) {
				Console.WriteLine("Dies ist der {0}. eingefügte Datensatz mit dem Wert: \"{1}\"", reader[0].ToString(), reader[1].ToString());
			}
			// Freigabe der Ressourcen.
			command.Dispose();
			
		}

		public List<MRListData> getMovieList(MovieEnum editable) {
			SQLiteCommand command = new SQLiteCommand(connection);
			List<MRListData> movieList = new List<MRListData>();
            command.CommandText = "SELECT idMovies, title, year, " + editable.ToString() + ", posterPath FROM Movies";
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				movieList.Add(new MRListData(int.Parse(reader[0].ToString()), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString(), reader[4].ToString()));
			}
			command.Dispose();
			return movieList;
		}
        
        public List<MRListData> getPersonList(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MRListData> personList = new List<MRListData>();
            List<string> personID = new List<string>();
            command.CommandText = "SELECT Person_idPerson, CharacterName, FROM Role WHERE Movies_idMovies = " + idMovies;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                personID.Add(reader[0].ToString());
            }
            return personList;
        }

        public GUIMovie getMovieInfo(int idMovies)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM Movies WHERE idMovies = " + idMovies;
            SQLiteDataReader reader = command.ExecuteReader();
            Console.WriteLine(reader[1]);
             return new GUIMovie();
        }

		public bool testAndSetPerson(uint imdbID) {
			bool contains;
			lock (this.imdbPersonIds) {
				contains = this.imdbPersonIds.Contains(imdbID);
				if (!contains) {
					this.imdbPersonIds.Add(imdbID);
					SQLiteCommand command = new SQLiteCommand(connection);
					command.CommandText = "insert into Person (imdbID) values (" + imdbID.ToString() + ")";
					command.ExecuteNonQuery();
				}
			}
			return contains;

		}
		public void closeConnection() {
			connection.Dispose();
			connection.Close();
		}
		public void insertMovieData(Movie m) {
			Console.WriteLine("done");
		}
	}
}
