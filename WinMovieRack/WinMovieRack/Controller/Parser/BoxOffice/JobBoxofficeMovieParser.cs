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
		private const string genreRegex = @"<div class=""mp_box_tab"">Genres</div>(?<genre>(.|\n|\r)*?)</table";


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
				extractFirstWeekend();
				extractGenres();
				extractFranchises();
			}
			if (foreignPage != null) {

			}
			if (weekendPage != null) {
			
			}
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
		}

		private void extractFranchises() {

		}

	}
}
