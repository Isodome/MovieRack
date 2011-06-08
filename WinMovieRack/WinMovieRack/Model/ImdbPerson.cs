using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WinMovieRack.Model {
	public class ImdbPerson {

		public uint imdbID;
		public string name;
		public DateTime birthday;
		public string birthname;
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
			Console.WriteLine("Birthname: " + this.birthname);
		}
	}
}
