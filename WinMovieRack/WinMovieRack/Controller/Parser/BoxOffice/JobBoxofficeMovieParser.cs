using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;
using System.Text.RegularExpressions;
using WinMovieRack.Model.DataTypes;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class JobBoxofficeMovieParser :ThreadJob {

		private const string openingWeekendRegex = @"Weekend:</a></td><td>(?<we>(.|\n|\r)*?)</td>";
		private const string lifeTimeGrossesTableRegex = @"<div class=""mp_box_tab"">Total Lifetime Grosses</b></div>(?<num>(.|\n|\r)*?)</table";
		//private const string domesticSummaryTableRegex = @"<div class=""mp_box_tab"">Domestic Summary</b></div>(?<dom>(.|\n|\r)*?)</table";
		private const string genreRegex = @"<div class=""mp_box_tab"">Genres</div>(?<genre>(.|\n|\r)*?)</table";
		private const string franchiseRegex = @"<div class=""mp_box_tab"">Franchises</div>(?<franchise>(.|\n|\r)*?)</table";
		private const string tableCellRegex = @"<td(.|\n|\r)*?</td>";
		private const string tableRowRegex = @"<tr(.|\n|\r)*?</tr>";
		private const string genreIDNameRegex = @"<a href=""/genres/chart/\?id=(?<id>.*?)\.htm"">(?<name>.*?)</a>";
		private const string franchiseIDNameRegex = @"<a href=""/showdowns/chart/\?id=(?<id>.*?)\.htm"">(?<name>.*?)</a>";
		private const string foreignTableRegex = @"<font size=""2"">View:(?<foreign>(.|\n|\r)*?)</table";

		private string mainPage;
		private string weekendPage;
		private string foreignPage;

		private BoxofficeMovie movie;
		public JobBoxofficeMovieParser(string mainpage, string foreignPage, string weekendPage, BoxofficeMovie movie) {
			this.mainPage = mainpage;
			this.weekendPage = weekendPage;
			this.foreignPage = foreignPage;
			this.movie = movie;
		}

		public void run() {
			if (mainPage != null) {
				extractLifetimeGrosses();
				extractFirstWeekend();
				extractGenres();
				extractFranchises();
			}
			if (foreignPage != null) {
				extractForeign();
			}
			if (weekendPage != null) {
			
			}
		}

		private void extractForeign() {
			Match m = Regex.Match(foreignPage, foreignTableRegex);
			if (!m.Success) {
				return;
			}
			string table = m.Groups["foreign"].Value;
			MatchCollection mRows = Regex.Matches(table, tableRowRegex);
			int countryCol = 0;
			int moneyCol = 2;

			string tmp = mRows[0].Value;
			MatchCollection findCols = Regex.Matches(tmp, tableCellRegex);
			for (int j = 0; j < findCols.Count; j++) {
				string head = findCols[j].Value;
				if (head.Contains("Country")) {
					countryCol = j;
				} else if (head.Contains("Total Gross")) {
					moneyCol = j;
				}
			}

			for (int i = 3 ; i < mRows.Count ; i++) {
				string row = mRows[i].Value;
				MatchCollection mCols = Regex.Matches(row, tableCellRegex);

				string country = mCols[countryCol].Value;
				country = Regex.Replace(country, @"<.*?>", "").Trim();

				string finalString = mCols[moneyCol +1].Value;
				bool isFinal = finalString.Contains("Final");

				string moneystring = mCols[moneyCol].Value;
				moneystring = Regex.Replace(moneystring, @"<.*?>", "").Trim();
				moneystring = Regex.Replace(moneystring, @"\D+", "").Trim();
				Int64 money;
				if (Int64.TryParse(moneystring, out money)) {
					movie.foreign.Add(new BOForeignInfo(country, money, isFinal));
				}
				

				
				
			}
		}

		private void extractLifetimeGrosses() {
			movie.america = Symbols.NO_BO_NUMBER;
			movie.foreignTotal = Symbols.NO_BO_NUMBER;
			movie.worldwide = Symbols.NO_BO_NUMBER;

			Match m = Regex.Match(mainPage, lifeTimeGrossesTableRegex);
			string tmpstring = m.Groups["num"].Value;
			MatchCollection mc = Regex.Matches(tmpstring, tableCellRegex);
			char next= 'n';
			for (int i=0; i< mc.Count ; i++) {
				string cell = mc[i].Value;
				if (Regex.Match(cell, @"Domestic:").Success) {
					next = 'd';
				} else if (Regex.Match(cell, @"Foreign:").Success) {
					next = 'f';
				} else if (Regex.Match(cell, @"Worldwide:").Success) {
					next = 's';
				} else {
					string num = Regex.Replace(cell, @"<.*?>", "");
					 num = Regex.Replace(num, @"\D+", "");
					if (next == 'd') {
						if (!Int64.TryParse(num, out movie.america)) {
							movie.america = Symbols.NO_BO_NUMBER;
						}
						next = 'n';
					} else if (next == 'f') {
						if (!Int64.TryParse(num, out movie.foreignTotal)) {
							movie.foreignTotal = Symbols.NO_BO_NUMBER;
						}
						next= 'n';
					} else if (next == 's') {
						if (!Int64.TryParse(num, out movie.worldwide)) {
							movie.worldwide = Symbols.NO_BO_NUMBER;
						}
						next= 'n';
					}
				}
			}
			//TODO
		}

		private void extractFirstWeekend() {
			Match m = Regex.Match(mainPage, openingWeekendRegex);
			string tmpstring = m.Groups["we"].Value;
			tmpstring = Regex.Replace(tmpstring, @"\D+", "");
			if (!Int64.TryParse(tmpstring, out movie.openingWeekend)) {
				movie.openingWeekend = Symbols.NO_BO_NUMBER;
			}
		}

		private void extractGenres() {
			Match m = Regex.Match(mainPage, genreRegex);
			if (!m.Success) {
				return;
			}
			string genreTable = m.Groups["genre"].Value;
			MatchCollection mc = Regex.Matches(genreTable, tableCellRegex);


			for (int i = 0 ; i < mc.Count ; i++) {
				string cell = mc[i].Value;
				Match match = Regex.Match(cell, genreIDNameRegex);
				if (match.Success) {
					int rank;
					string nextName = match.Groups["name"].Value.Trim();
					nextName = Regex.Replace(nextName, @"<.*?>", "").Trim();
					string nextID = match.Groups["id"].Value;
					i++;
					cell = mc[i].Value;
					string rankstring = Regex.Replace(cell, @"<.*?>", "").Trim();
					if (int.TryParse(rankstring, out rank)) {
						movie.genres.Add(new BOGenre(nextName, nextID, rank));
					}
				}
			}
		}

		private void extractFranchises() {
			Match m = Regex.Match(mainPage, franchiseRegex);
			if (!m.Success) {
				return;
			}
			string genreTable = m.Groups["franchise"].Value;
			MatchCollection mc = Regex.Matches(genreTable, tableCellRegex);


			for (int i = 0 ; i < mc.Count ; i++) {
				string cell = mc[i].Value;
				Match match = Regex.Match(cell, franchiseIDNameRegex);
				if (match.Success) {
					int rank;
					string nextName = match.Groups["name"].Value.Trim();
					nextName = Regex.Replace(nextName, @"<.*?>", "").Trim();
					string nextID = match.Groups["id"].Value;
					i++;
					cell = mc[i].Value;
					string rankstring = Regex.Replace(cell, @"<.*?>", "").Trim();
					if (int.TryParse(rankstring, out rank)) {
						movie.franchises.Add(new BOFranchise(nextName, nextID, rank));
					}
				}
			}
		}

	}
}
