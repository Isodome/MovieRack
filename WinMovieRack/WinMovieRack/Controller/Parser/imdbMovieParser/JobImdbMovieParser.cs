using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Drawing;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.Controller.Parser.imdbMovieParser {
	delegate void parseFunctions();
	public class JobImdbMovieParser : ThreadJob {

		// Regex to parse information from the imdbMainPage
		public const string titleAndYearRegex = @"<title>(.*)\((.*?)\) - IMDb</title>";
		public const string plotRegex = @"<h2>Storyline</h2>(.|\n|\r)*?<p>(?<plot>(.|\n|\r)*?)</p>";
		public const string writtenByRegex = @"<(.|\n|\r)*?>";
		public const string runtimeRegex = @"<h4 class=""inline"">Runtime:</h4>(.|\n|\r)*?(?<time>\d+) min";
		public const string originalTitleRegex = @"<br\s/><span\sclass=""title-extra"">(?<orgTitle>(.|\n|\r)*?)<i>\(original\stitle\)</i>";
		public const string genresRegex = @"<div class=""see-more inline canwrap"">(.|\n|\r)*?<h4 class=""inline"">Genres:</h4>(?<genres>(.|\n|\r)*?)</div>";
		public const string getLinkTextRegex = "<a href=.*?>(?<g>.*?)</a>";
		public const string imdbRatingRegex = @"<span class=""rating-rating"">(?<rating>.*?)<span";
		public const string imdbRatingVotesRegex = @"Users:(.|\n|\r)*?href=""ratings""(.|\n|\r)*?>(?<votes>.*?)votes</a>\)";
		public const string countriesRegex = @"<h4 class=""inline"">Country:</h4>(?<country>(.|\n|\r)*?)</div>";
		public const string languagesRegex = @"<h4 class=""inline"">Language:</h4>(?<lang>(.|\n|\r)*?)</div>";
		public const string alsoKnownAsRegex = @"<h4 class=""inline"">Also Known As:</h4>(?<ana>.*)";
		public const string awardsTableRegex = @"<table style=""margin-top: 8px; margin-bottom: 8px"" cellspacing=""2"" cellpadding=""2"" border=""1"">(?<bigtable>(.|\r|\n)*?)</table>";
		public const string awardWinnerRegex = @"<a href=""/name/nm(?<nm>\d+?)/";
		public const string starsRegex = @"<h4 class=""inline"">Stars:</h4>(?<stars>(.|\n|\r)*?)</div>";
		public const string getStarRegex = @"href=""/name/nm(?<g>\d+)/""";

		//Regex to parse information from the imdb credits page
		public const string directorsRegex = @">Directed by</a>(?<directors>.*?)Writing credits<";
		public const string writersRegex = @">Writing credits</a>(?<writers>.*?)</table>";
		public const string castRegex = @"<table class=""cast"">(?<cast>(.|\n|\r)*?)</table>";
		public const string id_RoleRegex = @"<td class=""nm""><a href=""/name/nm(?<nm>\d+?)/(.|\n|\r)*?</td><td class=""char"">(?<char>.*?)</td>";
		public const string getNameFromURLRegex = @"<a href=""/name/nm(?<g>\d+)/"">";

		private ImdbMovie movie;

		private string mainPage;
		private string creditsPage;
		private string awardsPage;

		parseFunctions startSelectiveParse;

		/// <summary>
		/// Creates a new Parse job. All 3 html string are allowed to be null. 
		/// </summary>
		/// <param name="mainPage">Needed for title, year, runtime, original title, genres, imdbrating, imdbvotes, languages, countries, also known as.</param>
		/// <param name="creditsPage">Needed for directors, writers, cast</param>
		/// <param name="awardsPage"></param>
		public JobImdbMovieParser(string mainPage, string creditsPage, string awardsPage) {
			this.initialize(mainPage, creditsPage, awardsPage, new ImdbMovie());
		}

		public JobImdbMovieParser(string mainPage, string creditsPage, string awardsPage, ImdbMovie movieToFillOut) {
			this.initialize(mainPage, creditsPage, awardsPage, movieToFillOut);
		}

		private void initialize(string mainPage, string creditsPage, string awardsPage, ImdbMovie movieToFillOut) {

			this.movie = movieToFillOut;
			this.mainPage = mainPage;
			this.creditsPage = creditsPage;
			this.awardsPage = awardsPage;

			if (mainPage != null) {
				startSelectiveParse += this.extractTitleAndYear;
				startSelectiveParse += this.extractPlot;
				startSelectiveParse += this.extractRuntime;
				startSelectiveParse += this.extractOriginalTitle;
				startSelectiveParse += this.extractGenres;
				startSelectiveParse += this.extractIMDBRating;
				startSelectiveParse += this.extractIMDBRatingVotes;
				startSelectiveParse += this.extractLanguages;
				startSelectiveParse += this.extractCountries;
				startSelectiveParse += this.extractAlsoKnownAs;
				startSelectiveParse += this.extractStars;
			}
			if (creditsPage != null) {
				startSelectiveParse += this.extractDirectors;
				startSelectiveParse += this.extractWriters;
				startSelectiveParse += this.extractCast;
			}
			if (awardsPage != null) {
				startSelectiveParse += extractAwards;
			}
		}

		public void run() {
			startSelectiveParse();
		}
		public ImdbMovie getResult() {
			return (movie);
		}

		public void extractTitleAndYear() {
			Match m = Regex.Match(mainPage, titleAndYearRegex);
			movie.title = m.Groups[1].Value.Trim();
			string yearstring = Regex.Replace(m.Groups[2].Value, @"\D*", "");
			movie.year = int.Parse(yearstring);

		}
		public void extractPlot() {
			Match m = Regex.Match(mainPage, plotRegex);
			movie.plot = m.Groups["plot"].Value;
			movie.plot = Regex.Replace(movie.plot, writtenByRegex, "").Trim(); // Remove all html tags
		}
		public void extractRuntime() {
			Match m = Regex.Match(mainPage, runtimeRegex);
			if (int.TryParse(m.Groups["time"].Value, out movie.runtime)) {
				movie.runtime = Symbols.NO_RUNTIME;
			}


		}
		public void extractOriginalTitle() {
			Match m = Regex.Match(mainPage, originalTitleRegex);
			movie.originalTitle = m.Groups["orgTitle"].Value.Trim();
		}
		public void extractGenres() {
			Match m = Regex.Match(mainPage, genresRegex);
			string genreString = m.Groups["genres"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc) {
				movie.genres.Add(match.Groups["g"].Value.Trim());
			}
		}
		public void extractIMDBRating()
		{
			Match m = Regex.Match(mainPage, imdbRatingRegex);
			string tmpString =m.Groups["rating"].Value;
			if (!int.TryParse(Regex.Replace(m.Groups["rating"].Value, @"\D", ""), out movie.imdbRating)) {
				movie.imdbRating = Symbols.NO_IMDB_RATING;
			}

		}

		public void extractIMDBRatingVotes() {
			Match m = Regex.Match(mainPage, imdbRatingVotesRegex);
			string tmpString = Regex.Replace(m.Groups["votes"].Value, @"\D", "");

			if(!int.TryParse(tmpString, out movie.imdbRatingVotes)) {
				movie.imdbRatingVotes = Symbols.NO_IMDB_VOTES;
			}
		}

		public void extractCountries() {
			Match m = Regex.Match(mainPage, countriesRegex);
			string genreString = m.Groups["country"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc) {
				movie.countries.Add(match.Groups["g"].Value.Trim());
			}
		}
		public void extractLanguages() {
			Match m = Regex.Match(mainPage, languagesRegex);
			string genreString = m.Groups["lang"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc) {
				movie.languages.Add(match.Groups["g"].Value.Trim());
			}
		}
		public void extractAlsoKnownAs() {
			Match m = Regex.Match(mainPage, alsoKnownAsRegex);
			movie.alsoKnownAs = m.Groups["ana"].Value.Trim();
		}
		public void extractDirectors() {
			Match m = Regex.Match(creditsPage, directorsRegex);
			string tmp = m.Groups["directors"].Value;

			MatchCollection mc = Regex.Matches(tmp, getNameFromURLRegex);
			foreach (Match match in mc) {
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				uint nm = uint.Parse(id);
				if (!movie.directors.Contains<uint>(nm)) {
					movie.directors.Add(nm);
				}
			}

		}
		public void extractWriters() {
			Match m = Regex.Match(creditsPage, writersRegex);
			string tmp = m.Groups["writers"].Value;
			MatchCollection mc = Regex.Matches(tmp, getNameFromURLRegex);
			foreach (Match match in mc) {
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				uint nm = uint.Parse(id);
				if (!movie.writers.Contains<uint>(nm)) {
					movie.writers.Add(nm);
				}
			}
		}
		public void extractCast() {
			Match m = Regex.Match(creditsPage, castRegex);
			string tmp = m.Groups["cast"].Value;

			MatchCollection mc = Regex.Matches(tmp, id_RoleRegex);
			foreach (Match match in mc) {
				string nmstring = match.Groups["nm"].Value.Trim();
				string role = match.Groups["char"].Value.Trim();
				role = Regex.Replace(role, @"<.*?>", ""); // Remove link if there is one
				uint nm = uint.Parse(nmstring);
				movie.roles.Add(Tuple.Create<uint, string>(nm, role));
			}
		}
		public void extractStars() {
			Match m = Regex.Match(mainPage, starsRegex);
			string tmp = m.Groups["stars"].Value;

			MatchCollection mc = Regex.Matches(tmp, getStarRegex);
			foreach (Match match in mc) {
				string nmstring = match.Groups["g"].Value.Trim();
				uint nm = uint.Parse(nmstring);
				if (!movie.stars.Contains<uint>(nm)) {
					movie.stars.Add(nm);
				}
			}
		}


		public void extractAwards() {
			Match mTable = Regex.Match(awardsPage, awardsTableRegex);
			string awardsTable = mTable.Groups["bigtable"].Value;

			int[] rowSpanRemaining = {0,0,0,0};
			int currentColumn = 0;
			int currentYear = 0;
			string currentAward = null;
			string currentResult = null;

			MatchCollection mc = Regex.Matches(awardsTable, @"(?<cell><td(.|\n|\r)*?</td>)");
			foreach (Match match in mc) {
				string curCell = match.Groups["cell"].Value;
				if (Regex.Match(curCell, @"colspan=""4""").Success) {
					continue;
				} else {
					while (rowSpanRemaining[currentColumn] > 0) {
						rowSpanRemaining[currentColumn]--;
						currentColumn = ++currentColumn % 4;
					}

					if (currentColumn != 3) {
						Match m = Regex.Match(curCell, @"rowspan=""(?<rowspan>\d+)""");
						int rs = int.Parse(m.Groups["rowspan"].Value);

						string content = Regex.Replace(curCell, @"(<.*?>|\n)", "").Trim();

						rowSpanRemaining[currentColumn] = rs - 1;

						if (currentColumn == 0) {
							if (!int.TryParse(content, out currentYear)) {
								currentYear = -1;
							}
						} else if (currentColumn == 1) {
							currentResult = content;
						} else if (currentColumn == 2) {
							currentAward = content;
						}
						currentColumn++;
					} else {
						if (currentYear != -1) {
							addAward(curCell, currentYear, currentAward, currentResult);
						}
						currentColumn = 0;
					}

				}
			}
			

		}

		private void addAward(string text, int year, string award, string result) {
			Match m = Regex.Match(text, @"<td valign=""top"">(?<category>(.|\n|\r)*?)<br>");
			string tmpcategory = m.Groups["category"].Value;
			string category = Regex.Replace(tmpcategory, @"(<.*?>|\n)", "").Trim();

			Award a = new Award();
			a.isOscar = Regex.Match(award, @"Oscar").Success;
			a.won = Regex.Match(result, @"Won").Success;
			a.year = year;
			a.award = award;
			a.category = category;

			MatchCollection mc = Regex.Matches(text, awardWinnerRegex);
			
			foreach (Match match in mc) {
				string nmstring = match.Groups["nm"].Value.Trim();
				uint nm = uint.Parse(nmstring);

				a.persons.Add(nm);
			}
			movie.awards.Add(a);
		}
	}
}