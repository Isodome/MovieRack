using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WinMovieRack.Controller.Parser;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class BoxOfficeSearcher {

		private const string boxOfficeSearchAdress = "http://boxofficemojo.com/search/?q={0}";
		private const string movieMatchesRegex = @"Movie Matches: </b><table(.|\r|\n)*?</table>";
		private const string tableRowRegex = @"<tr(.|\r|\n)*?</tr>";
		private const string boxofficeIdRegex = @"size=""2""><a href=""/movies/.id=(?<id>.*?).htm"">(?<name>.*?)</a>";
		private const string releaseDateRegex = @"<a href=""/schedule/.*?"">(?<date>.*?)</a>";
		private const string coverURLRegex = @"<img src=""(?<url>.*?)""";
		private const string urlRegex = @"boxofficemojo.*?id=(?<id>.*?).htm";

		private string query = null;
		private string website = null;
		BoxOfficeSearchResultCollection result;

		public BoxOfficeSearcher(string searchFor){
			query = searchFor;
			result = new BoxOfficeSearchResultCollection(searchFor);
		}

		public BoxOfficeSearchResultCollection searchOnBoxOffice() {

			JobWebPageDownload downloadJob = new JobWebPageDownload(String.Format(boxOfficeSearchAdress, query));
			downloadJob.run();
			website = downloadJob.getResult();

			if (website != null) {
				extractMovieMatches();
				extractPersonMatches();
			}
			return result;
		}

		private void extractPersonMatches() {
 			
		}

		private void extractMovieMatches() {
			Match match = Regex.Match(website, movieMatchesRegex);
			if (!match.Success) {
				return;
			}
			string table = match.Value;
			MatchCollection rowMatches = Regex.Matches(table, tableRowRegex);
			foreach (Match rowMatch in rowMatches) {
				BoxOfficeSearchResult r = new BoxOfficeSearchResult();

				string row = rowMatch.Value;

				Match idMatch =  Regex.Match(row, boxofficeIdRegex);
				r.boxofficeID = idMatch.Groups["id"].Value;
				r.name = idMatch.Groups["name"].Value.Trim();

				Match releaseDateMatch = Regex.Match(row, releaseDateRegex);
				r.release = releaseDateMatch.Groups["date"].Value;

				Match imgMatch = Regex.Match(row, coverURLRegex);
				if (imgMatch.Success) {
					JobLoadImage job = new JobLoadImage(imgMatch.Groups["url"].Value, null);
					job.run();
					r.cover = job.getResult();
				}

				if (r.name != null && !r.name.Trim().Equals(string.Empty) && r.boxofficeID != null && !r.boxofficeID.Trim().Equals(string.Empty)) {
					result.addMovieMatch(r);
				}

			}
		}

		public static string getBoxofficeIdFromURL(string url) {
			Match m = Regex.Match(url, urlRegex);
			if (m.Success) {
				return m.Groups["id"].Value;
			} else {
				return null;
			}
		}

	}

}

