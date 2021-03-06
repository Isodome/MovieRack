﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model.DataTypes;
namespace WinMovieRack.Model {
	public class BoxofficeMovie {

		public string boxofficeid;
		public Int64 worldwide;
		public Int64 america;
		public Int64 foreignTotal;
		public Int64 openingWeekend;
		public int rankFirstWeekend;
		public int rankAllTime;
		public int weeksInCinema;
		public List<BOFranchise> franchises;
		public List<BOGenre> genres;
		public List<BOForeignInfo> foreign;

		public BoxofficeMovie(string id) {
			this.boxofficeid = id;
			genres = new List<BOGenre>();
			franchises = new List<BOFranchise>();
			foreign = new List<BOForeignInfo>();
		}

		public void printToConsole() {
			Console.WriteLine("ID: '{0}'", boxofficeid);
			Console.WriteLine("Worldwide: ${0}", worldwide);
			Console.WriteLine("America: ${0}", america);
			Console.WriteLine("Foreign: ${0}", foreignTotal);
			Console.WriteLine("Opening Weekend: ${0}", openingWeekend);
			Console.WriteLine("rankFirstWeekend: ${0}", rankFirstWeekend);
			Console.WriteLine("rankAllTime: ${0}", rankAllTime);
			Console.WriteLine("weeksInCinema: ${0}", weeksInCinema);
			Console.WriteLine("Franchises: ");
			foreach (BOFranchise french in franchises) {
				Console.WriteLine("Name: '{1}', ID: '{0}',  Rank: {2}", french.id, french.name, french.rank);
			}
			Console.WriteLine("Genres: ");
			foreach (BOGenre gen in genres) {
				Console.WriteLine("Name: '{1}', ID: '{0}',  Rank: {2}", gen.id, gen.name, gen.rank);
			}
			Console.WriteLine("Foreign: ");
			foreach (BOForeignInfo info in foreign) {
				Console.WriteLine("Country: '{0}', Money: '{1}', isFinal: {2}", info.country, info.money, info.isFinal);
			}
		}
	}
}
