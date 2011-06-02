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
    class JobPrintResults : ThreadJob
    {
        private imdbMovieParserMaster parent;
        public JobPrintResults(imdbMovieParserMaster parent)
        {
            this.parent = parent;
        }

        public void run()
        {
            printResults();
            parent.hasFinished(this);
        }

        /*
		 * For debug only
		 */
		private void printResults()
		{
			Console.WriteLine("imdbID: " + parent.imdbID);
			Console.WriteLine("Title: " + parent.title);
			Console.WriteLine("Original Title: " + parent.originalTitle);
			Console.WriteLine("Year:" + parent.year);
			Console.WriteLine("Runtime: " + parent.runtime);
			Console.WriteLine("Plot: " + parent.plot);
			Console.Write("Genres: ");
			foreach (string s in parent.genres)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.WriteLine("imdb Rating(*10): " + parent.imdbRating);
			Console.WriteLine("Votes: " + parent.imdbRatingVotes);
			Console.Write("Countries: ");
			foreach (string s in parent.countries)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.Write("Languages: ");
			foreach (string s in parent.languages)
			{
				Console.Write(s + ", ");
			}
			Console.WriteLine("");
			Console.WriteLine("Also known as: " + parent.alsoKnownAs);
			Console.Write("Directors: ");
			foreach (uint s in parent.directors)
			{
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("");
			Console.Write("Writers: ");
			foreach (uint s in parent.writers)
			{
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("");

			Console.Write("Cast: ");
			foreach (Tuple<uint,string> t in parent.roles)
			{
				Console.WriteLine(t.Item1.ToString() + " as " + t.Item2);
			}
		}

    }
}
