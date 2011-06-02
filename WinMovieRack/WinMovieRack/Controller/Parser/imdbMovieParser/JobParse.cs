using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Drawing;

namespace WinMovieRack.Controller.Parser
{
    class JobParse : ThreadJob
    {
        private imdbMovieParserMaster parent;
        public JobParse(imdbMovieParserMaster parent)
        {
            this.parent = parent;
        }

        public void run()
        {
            doParse();
        }

		public void doParse()
		{
			extractTitleAndYear();
			extractPlot();
			extractRuntime();
			extractOriginalTitle();
			extractGenres();
			extractIMDBRating();
			extractIMDBRatingVotes();
			extractLanguages();
			extractCountries();
			extractAlsoKnownAs();
			extractDirectors();
			extractWriters();
			extractCast();
        }

        private void extractTitleAndYear()
        {
            Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.titleAndYearRegex);
            parent.title = m.Groups[1].Value.Trim();
            parent.year = int.Parse(m.Groups[2].Value);

        }
        private void extractPlot()
        {
            Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.plotRegex);
            parent.plot = m.Groups["plot"].Value;
            parent.plot = Regex.Replace(parent.plot, imdbMovieParserMaster.writtenByRegex, "").Trim(); // Remove all html tags
        }
        private void extractRuntime()
        {
            Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.runtimeRegex);
            parent.runtime = int.Parse( m.Groups["time"].Value);

        }
        private void extractOriginalTitle()
        {
            Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.originalTitleRegex);
            parent.originalTitle = m.Groups["orgTitle"].Value.Trim();
        }
		private void extractGenres()
		{
			Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.genresRegex);
			string genreString = m.Groups["genres"].Value;
			MatchCollection mc = Regex.Matches(genreString, imdbMovieParserMaster.getLinkTextRegex);
			foreach (Match match in mc)
			{
				parent.genres.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractIMDBRating()
		{
			Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.imdbRatingRegex);
			string tmpString = Regex.Replace(m.Groups["rating"].Value, @"\D", "");
			parent.imdbRating = int.Parse(tmpString);
			
		}
		private void extractIMDBRatingVotes()
		{
			Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.imdbRatingVotesRegex);
			string tmpString = Regex.Replace(m.Groups["votes"].Value, @"\D", "");
			parent.imdbRatingVotes = int.Parse(tmpString);
		}
		private void extractCountries() 
		{
			Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.countriesRegex);
			string genreString = m.Groups["country"].Value;
			MatchCollection mc = Regex.Matches(genreString, imdbMovieParserMaster.getLinkTextRegex);
			foreach (Match match in mc)
			{
				parent.countries.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractLanguages()
		{
			Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.languagesRegex);
			string genreString = m.Groups["lang"].Value;
			MatchCollection mc = Regex.Matches(genreString, imdbMovieParserMaster.getLinkTextRegex);
			foreach (Match match in mc)
			{
				parent.languages.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractAlsoKnownAs()
		{
			Match m = Regex.Match(parent.mainPage, imdbMovieParserMaster.alsoKnownAsRegex);
			parent.alsoKnownAs = m.Groups["ana"].Value.Trim();
		}
		private void extractDirectors() 
		{
			Match m = Regex.Match(parent.creditsPage, imdbMovieParserMaster.directorsRegex);
			string tmp = m.Groups["directors"].Value;
			
			MatchCollection mc = Regex.Matches(tmp, imdbMovieParserMaster.getNameFromURLRegex);
			foreach (Match match in mc)
			{
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				parent.directors.Add(uint.Parse(id));
			} 

		}
		private void extractWriters()
		{
			Match m = Regex.Match(parent.creditsPage, imdbMovieParserMaster.writersRegex);
			string tmp = m.Groups["writers"].Value;
			MatchCollection mc = Regex.Matches(tmp, imdbMovieParserMaster.getNameFromURLRegex);
			foreach (Match match in mc)
			{
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				parent.writers.Add(uint.Parse(id));
			} 
		}
		private void extractCast()
		{
			Match m = Regex.Match(parent.creditsPage, imdbMovieParserMaster.castRegex);
			string tmp = m.Groups["cast"].Value;

			MatchCollection mc = Regex.Matches(tmp, imdbMovieParserMaster.id_RoleRegex);
			foreach (Match match in mc)
			{
				string nmstring = match.Groups["nm"].Value.Trim();
				string role = match.Groups["char"].Value.Trim();
				role = Regex.Replace(role, @"<.*?>", ""); // Remove link if there is one
				uint nm = uint.Parse(nmstring);
				parent.roles.Add(Tuple.Create<uint, string>(nm, role));
			} 
		}
    }
}