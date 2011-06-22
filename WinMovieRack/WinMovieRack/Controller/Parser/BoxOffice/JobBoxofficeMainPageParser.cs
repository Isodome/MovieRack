using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;
using System.Text.RegularExpressions;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class JobBoxofficeMainPageParser :ThreadJob {

		private const string genreRegex = @"<div class=""mp_box_tab"">Genres</div>(?<genre>(.|\n|\r)*?)</table";


		private string mainPage;
		private BoxofficeMovie movie;
		public JobBoxofficeMainPageParser(string website,BoxofficeMovie movie) {
			this.mainPage = website;
			this.movie = movie;
		}

		public void run() {
			if (mainPage != null) {
				extractGenres();
				extractFranchises();
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
			throw new NotImplementedException();
		}

	}
}
