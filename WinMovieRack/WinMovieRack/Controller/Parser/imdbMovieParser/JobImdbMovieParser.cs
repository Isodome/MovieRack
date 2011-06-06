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

namespace WinMovieRack.Controller.Parser.imdbMovieParser
{
	delegate void parseFunctions();
    public class JobImdbMovieParser : ThreadJob
    {

		// Regex to parse information from the imdbMainPage
		public const string titleAndYearRegex = @"<title>(.*?)\((\d+)\) - IMDb</title>";
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
		public JobImdbMovieParser(string mainPage, string creditsPage, string awardsPage)
        {
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
			}
			if (creditsPage != null) {
				startSelectiveParse += this.extractDirectors;
				startSelectiveParse += this.extractWriters;
				startSelectiveParse += this.extractCast;
			}
			if (awardsPage != null) {
			}
		}

        public void run()
        {
			startSelectiveParse();
        }
		public ImdbMovie getResult() {
			return (movie);
		}

        public void extractTitleAndYear()
        {
            Match m = Regex.Match(mainPage, titleAndYearRegex);
            movie.title = m.Groups[1].Value.Trim();
            movie.year = int.Parse(m.Groups[2].Value);

        }
        public void extractPlot()
        {
            Match m = Regex.Match(mainPage, plotRegex);
            movie.plot = m.Groups["plot"].Value;
            movie.plot = Regex.Replace(movie.plot, writtenByRegex, "").Trim(); // Remove all html tags
        }
		public void extractRuntime()
        {
            Match m = Regex.Match(mainPage, runtimeRegex);
			try {
				movie.runtime = int.Parse(m.Groups["time"].Value);
			} catch (FormatException) {
				Log.log("Could not parse runtime of " + movie.title);
			}

        }
		public void extractOriginalTitle()
        {
            Match m = Regex.Match(mainPage, originalTitleRegex);
            movie.originalTitle = m.Groups["orgTitle"].Value.Trim();
        }
		public void extractGenres()
		{
			Match m = Regex.Match(mainPage, genresRegex);
			string genreString = m.Groups["genres"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				movie.genres.Add(match.Groups["g"].Value.Trim());
			}
		}
		public void extractIMDBRating()
		{
			Match m = Regex.Match(mainPage, imdbRatingRegex);
			string tmpString = Regex.Replace(m.Groups["rating"].Value, @"\D", "");
			movie.imdbRating = int.Parse(tmpString);
			
		}
		public void extractIMDBRatingVotes()
		{
			Match m = Regex.Match(mainPage, imdbRatingVotesRegex);
			string tmpString = Regex.Replace(m.Groups["votes"].Value, @"\D", "");

			try {
				movie.imdbRatingVotes = int.Parse(tmpString);
			} catch (FormatException) {
				Log.log("Could not parse imdb votes of " + movie.title);
				movie.imdbRatingVotes = -1;
			}
		}
		public void extractCountries() 
		{
			Match m = Regex.Match(mainPage, countriesRegex);
			string genreString = m.Groups["country"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				movie.countries.Add(match.Groups["g"].Value.Trim());
			}
		}
		public void extractLanguages()
		{
			Match m = Regex.Match(mainPage, languagesRegex);
			string genreString = m.Groups["lang"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				movie.languages.Add(match.Groups["g"].Value.Trim());
			}
		}
		public void extractAlsoKnownAs()
		{
			Match m = Regex.Match(mainPage, alsoKnownAsRegex);
			movie.alsoKnownAs = m.Groups["ana"].Value.Trim();
		}
		public void extractDirectors() 
		{
			Match m = Regex.Match(creditsPage, directorsRegex);
			string tmp = m.Groups["directors"].Value;
			
			MatchCollection mc = Regex.Matches(tmp, getNameFromURLRegex);
			foreach (Match match in mc)
			{
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				movie.directors.Add(uint.Parse(id));
			} 

		}
		public void extractWriters()
		{
			Match m = Regex.Match(creditsPage, writersRegex);
			string tmp = m.Groups["writers"].Value;
			MatchCollection mc = Regex.Matches(tmp, getNameFromURLRegex);
			foreach (Match match in mc)
			{
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				movie.writers.Add(uint.Parse(id));
			} 
		}
		public void extractCast()
		{
			Match m = Regex.Match(creditsPage, castRegex);
			string tmp = m.Groups["cast"].Value;

			MatchCollection mc = Regex.Matches(tmp, id_RoleRegex);
			foreach (Match match in mc)
			{
				string nmstring = match.Groups["nm"].Value.Trim();
				string role = match.Groups["char"].Value.Trim();
				role = Regex.Replace(role, @"<.*?>", ""); // Remove link if there is one
				uint nm = uint.Parse(nmstring);
				movie.roles.Add(Tuple.Create<uint, string>(nm, role));
			}
		}
    }
}