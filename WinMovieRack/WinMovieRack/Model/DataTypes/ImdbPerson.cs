using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinMovieRack.Model {
	public class ImdbPerson {

		public int idPerson;
		public uint imdbID;
		public string name;
		public DateTime birthday;
		public DateTime deathday;
		public string birthname;
		public char gender;
		public Bitmap image;

		public ImdbPerson() {

		}

		public ImdbPerson(uint id) : this() {
			this.imdbID = id;
		}

		public void printToConsole() {
			Console.WriteLine("IMDB ID: " + this.imdbID);
			Console.WriteLine("Name: " + this.name);
			Console.WriteLine("Birthday: " + this.birthday.ToString());
			Console.WriteLine("Deathday: " + this.deathday.ToString());
			Console.WriteLine("Birthname: " + this.birthname);
			Console.WriteLine("Gender: " + this.gender);
		}
	}
}
