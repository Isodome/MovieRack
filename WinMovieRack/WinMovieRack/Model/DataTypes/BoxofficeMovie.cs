using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model {
	public class BoxofficeMovie {

		public string boxofficeid;
		public Int64 worldwide;
		public Int64 america;
		public Int64 foreign;
		public Int64 openingWeekend;

		public BoxofficeMovie(string id) {
			this.boxofficeid = id;
		}

		public void printToConsole() {
			Console.WriteLine("ID: '{0}'", boxofficeid);
			Console.WriteLine("Worldwide: ${0}", worldwide);
			Console.WriteLine("America: ${0}", america);
			Console.WriteLine("Foreign: ${0}", foreign);
			Console.WriteLine("Opening Weekend: ${0}", openingWeekend);
		}
	}
}
