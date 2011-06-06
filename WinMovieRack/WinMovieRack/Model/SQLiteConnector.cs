using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
namespace WinMovieRack.Model {
	public class SQLiteConnector : DBInterface {
		private SQLiteConnection connection;

		public void initDb() {
			string dataSource = "MovieRack.db";
			connection = new SQLiteConnection();
			connection.ConnectionString = "Data Source=" + dataSource;
			connection.Open();

		}

		public void checkTables() {
			FileInfo file = new FileInfo("createscript.sql");
			string script = file.OpenText().ReadToEnd();

			SQLiteCommand command = new SQLiteCommand(connection);


			// Erstellen der Tabelle, sofern diese noch nicht existiert.
			command.CommandText = script;
			command.ExecuteNonQuery();

			// Einfügen eines Test-Datensatzes.
			command.CommandText = "INSERT INTO Movies (idMovies, Year, Title, Plot) VALUES(NULL, 1970, 'James Bond xx', 'laaangweilig')";
			command.ExecuteNonQuery();

			command.CommandText = "SELECT Plot, Title FROM Movies WHERE Year = 1970";

			SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read()) {
				Console.WriteLine("Dies ist der {0}. eingefügte Datensatz mit dem Wert: \"{1}\"", reader[0].ToString(), reader[1].ToString());
			}
			// Freigabe der Ressourcen.
			command.Dispose();

		}

        public List<MovieListData> getMovieList(MovieEnum editable)
        {
            SQLiteCommand command = new SQLiteCommand(connection);
            List<MovieListData> movieList = new List<MovieListData>();
            command.CommandText = "SELECT idMovies, title, imdbRating, " + editable.ToString() + ", posterPath FROM Movies";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                movieList.Add(new MovieListData(int.Parse(reader[0].ToString()), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString(), reader[4].ToString()));
            }
            command.Dispose();
            return movieList;
        }
	}
}
