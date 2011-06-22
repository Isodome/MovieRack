using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;
using System.Text.RegularExpressions;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class JobBoxofficeMovieParser :ThreadJob {

		private const string openingWeekendRegex = @"Weekend:</a></td><td>(?<we>(.|\n|\r)*?)</td>";
		private const string numbersTableRegex = @"<div class=""mp_box_tab"">Total Lifetime Grosses</b></div>(?<num>(.|\n|\r)*?)</table";
		private const string genreRegex = @"<div class=""mp_box_tab"">Genres</div>(?<genre>(.|\n|\r)*?)</table";
		private const string tableCellRegex = @"<td(.|\n|\r)*?</td?";


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
				extractNumbers();
				extractFirstWeekend();
				extractGenres();
				extractFranchises();
			}
			if (foreignPage != null) {

			}
			if (weekendPage != null) {
			
			}
		}

		private void extractNumbers() {
			Match m = Regex.Match(mainPage, numbersTableRegex);
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
						if (!Int64.TryParse(num, out movie.foreign)) {
							movie.foreign = Symbols.NO_BO_NUMBER;
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
			//TODO
		}

		private void extractFranchises() {

		}

	}
}
