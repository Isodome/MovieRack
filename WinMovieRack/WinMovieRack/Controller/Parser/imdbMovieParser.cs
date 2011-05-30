using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace WinMovieRack.Controller.Parser
{
    
    class imdbMovieParser
    {
        private const string placholder = "{ID}";
        private const string URL = "http://www.imdb.com/title/tt" + placholder + "/";

        private const string titleAndYearRegex = @"<title>(.*?)\((\d+)\) - IMDb</title>";
        private const string plotRegex = @"<h2>Storyline</h2>(.|\n|\r)*?<p>(?<plot>(.|\n|\r)*?)</p>";
        private const string writtenByRegex = @"<(.|\n|\r)*?>";
        private const string runtimeRegex = @"<h4 class=""inline"">Runtime:</h4>(.|\n|\r)*?(?<time>\d+) min";
		private const string originalTitleRegex = @"<br\s/><span\sclass=""title-extra"">(?<orgTitle>(.|\n|\r)*?)<i>\(original\stitle\)</i>";
		private const string genresRegex = @"<div class=""see-more inline canwrap"">(.|\n|\r)*?<h4 class=""inline"">Genres:</h4>(?<genres>(.|\n|\r)*?)</div>";
		private const string getLinkTextRegex = "<a href=.*?>(?<g>.*?)</a>";
		private const string imdbRatingRegex = @"<span class=""rating-rating"">(?<rating>.*?)<span";
		private const string countriesRegex = @"<h4 class=""inline"">Country:</h4>(?<country>(.|\n|\r)*?)</div>";
		private const string languagesRegex = @"<h4 class=""inline"">Language:</h4>(?<lang>(.|\n|\r)*?)</div>";
		private const string alsoKnownAsRegex = @"<h4 class=""inline"">Also Known As:</h4>(?<ana>.*)";


        private WebRequest req;

        private string src;

        private string imdbID;
        private string title;
        private int year;
        private int runtime;
        private string originalTitle;
		private List<string> genres;
        private string plot;
		private int imdbRating;
		private List<string> countries;
		private List<string> languages;
		private string alsoKnownAs;

        public imdbMovieParser(string imdbID) 
        {
            this.imdbID = imdbID;
            this.req = WebRequest.Create(Regex.Replace(URL, placholder, imdbID));
			genres = new List<string>();
			countries = new List<string>();
			languages = new List<string>();
        }

        public void doRequest()
        {
            WebResponse resp = req.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());
            src = r.ReadToEnd();
			
        }
		public void doParse()
		{
			extractTitleAndYear();
			extractPlot();
			extractRuntime();
			extractOriginalTitle();
			extractGenres();
			extractIMDBRating();
			extractLanguages();
			extractCountries();
			extractAlsoKnownAs();

			printResults();
		}


        private void extractTitleAndYear()
        {
            Match m = Regex.Match(src, titleAndYearRegex);
            this.title = m.Groups[1].Value.Trim();
            year = int.Parse(m.Groups[2].Value);

        }

        private void extractPlot()
        {
            Match m = Regex.Match(src, plotRegex);
            this.plot = m.Groups["plot"].Value;
            this.plot = Regex.Replace(plot, writtenByRegex, "").Trim(); // Remove all html tags
        }

        private void extractRuntime()
        {
            Match m = Regex.Match(src, runtimeRegex);
            this.runtime = int.Parse( m.Groups["time"].Value);

        }
        private void extractOriginalTitle()
        {
            Match m = Regex.Match(src, originalTitleRegex);
            this.originalTitle = m.Groups["orgTitle"].Value.Trim();
        }

		private void extractGenres()
		{
			Match m = Regex.Match(src, genresRegex);
			string genreString = m.Groups["genres"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				genres.Add(match.Groups["g"].Value.Trim());
			}
		}

		private void extractIMDBRating()
		{
			Match m = Regex.Match(src, imdbRatingRegex);
			string tmpString = Regex.Replace(m.Groups["rating"].Value, @"\D", "");
			imdbRating = int.Parse(tmpString);
			
		}

		private void extractCountries() 
		{
			Match m = Regex.Match(src, countriesRegex);
			string genreString = m.Groups["country"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				countries.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractLanguages()
		{
			Match m = Regex.Match(src, languagesRegex);
			string genreString = m.Groups["lang"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				languages.Add(match.Groups["g"].Value.Trim());
			}
		}

		private void extractAlsoKnownAs()
		{
			Match m = Regex.Match(src, alsoKnownAsRegex);
			this.alsoKnownAs = m.Groups["ana"].Value.Trim();
		}

		/*
		 * For debug only
		 */
		private void printResults()
		{
			Console.WriteLine("imdbID: " + this.imdbID);
			Console.WriteLine("Title: " + this.title);
			Console.WriteLine("Original Title: " + this.originalTitle);
			Console.WriteLine("Year:" + this.year);
			Console.WriteLine("Runtime: " + this.runtime);
			Console.WriteLine("Plot: " + this.plot);
			Console.Write("Genres: ");
			foreach (string s in genres)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.WriteLine("imdb Rating(*10): " + this.imdbRating);
			Console.Write("Countries: ");
			foreach (string s in countries)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.Write("Languages: ");
			foreach (string s in languages)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.WriteLine("Also known as: " + this.alsoKnownAs);
		}

    }

}
