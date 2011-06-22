using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model.DataTypes;
namespace WinMovieRack.Model {
	public class BoxofficeMovie {

		public string boxofficeid;
		public Int64 worldwide;
		public Int64 america;
		public Int64 foreign;
		public Int64 openingWeekend;
		public List<BOFranchise> franchises;
		public List<BOGenre> genres;

		public BoxofficeMovie(string id) {
			this.boxofficeid = id;
			genres = new List<BOGenre>();
			franchises = new List<BOFranchise>();
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
