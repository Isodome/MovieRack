using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model {
	public class BoxofficeMovie {

		private string boxofficeid;

		public BoxofficeMovie(string id) {
			this.boxofficeid = id;
		}

		public void printToConsole() {
			Console.WriteLine("ID: '{0}'", boxofficeid);
		}
	}
}
