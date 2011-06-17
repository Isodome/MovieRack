using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Controller.Parser.BoxOffice {

	public class BoxOfficeSearchResultCollection {

		private List<BoxOfficeSearchResult> movieMatches;
		private List<BoxOfficeSearchResult> personMatches;

		public string querystring {get; set;}

		public BoxOfficeSearchResultCollection(string query) {
			this.querystring = query;
			movieMatches = new List<BoxOfficeSearchResult>();
			personMatches = new List<BoxOfficeSearchResult>();
		}

		public void addMovieMatch(BoxOfficeSearchResult match) {
			movieMatches.Add(match);
		}

		public void addPersonMatch(BoxOfficeSearchResult match) {
			personMatches.Add(match);
		}

		public List<BoxOfficeSearchResult> getFoundPersons() {
			return this.personMatches;
		}
		public List<BoxOfficeSearchResult> getFoundMovies() {
			return this.movieMatches;
		}

	}
}
