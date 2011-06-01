using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Threading;
using System.Drawing;

namespace WinMovieRack.Controller.Parser
{
    
    class imdbMovieParser
    {
        private const string placholder = "{ID}";
        private const string URL = "http://www.imdb.com/title/tt" + placholder + "/";
		private const string URLAwards = URL + "awards";
		private const string URLcredits = URL + "fullcredits";


		// Regex to parse information from the imdbMainPage
        private const string titleAndYearRegex = @"<title>(.*?)\((\d+)\) - IMDb</title>";
        private const string plotRegex = @"<h2>Storyline</h2>(.|\n|\r)*?<p>(?<plot>(.|\n|\r)*?)</p>";
        private const string writtenByRegex = @"<(.|\n|\r)*?>";
        private const string runtimeRegex = @"<h4 class=""inline"">Runtime:</h4>(.|\n|\r)*?(?<time>\d+) min";
		private const string originalTitleRegex = @"<br\s/><span\sclass=""title-extra"">(?<orgTitle>(.|\n|\r)*?)<i>\(original\stitle\)</i>";
		private const string genresRegex = @"<div class=""see-more inline canwrap"">(.|\n|\r)*?<h4 class=""inline"">Genres:</h4>(?<genres>(.|\n|\r)*?)</div>";
		private const string getLinkTextRegex = "<a href=.*?>(?<g>.*?)</a>";
		private const string imdbRatingRegex = @"<span class=""rating-rating"">(?<rating>.*?)<span";
		private const string imdbRatingVotesRegex = @"Users:(.|\n|\r)*?href=""ratings""(.|\n|\r)*?>(?<votes>.*?)votes</a>\)";
		private const string countriesRegex = @"<h4 class=""inline"">Country:</h4>(?<country>(.|\n|\r)*?)</div>";
		private const string languagesRegex = @"<h4 class=""inline"">Language:</h4>(?<lang>(.|\n|\r)*?)</div>";
		private const string alsoKnownAsRegex = @"<h4 class=""inline"">Also Known As:</h4>(?<ana>.*)";
		

		//Regex to parse information from the imdb credits page
		private const string directorsRegex = @">Directed by</a>(?<directors>.*?)Writing credits<";
		private const string writersRegex = @">Writing credits</a>(?<writers>.*?)</table>";
		private const string castRegex = @"<table class=""cast"">(?<cast>(.|\n|\r)*?)</table>";
		private const string id_RoleRegex = @"<td class=""nm""><a href=""/name/nm(?<nm>\d+?)/(.|\n|\r)*?</td><td class=""char"">(?<char>.*?)</td>";
		private const string getNameFromURLRegex = @"<a href=""/name/nm(?<g>\d+)/"">";


        private WebRequest mainPageRequest;
		private WebRequest awardsPageRequest;
		private WebRequest creditsPageRequest;

		// Full text of websites;
        private string mainPage;
		private string awardsPage;
		private string creditsPage;

		private const string mediaURLRegex = @"id=""img_primary""(.|\n|\r)*?<img src=""(?<url>.*?V1).*?""";

        private uint imdbID;
        private string title;
        private int year;
        private int runtime;
        private string originalTitle;
		private List<string> genres;
        private string plot;
		private int imdbRating;
		private int imdbRatingVotes;
		private List<string> countries;
		private List<string> languages;
		private List<uint> directors;
		private List<uint> writers;
		private List<Tuple<uint, string>> roles;
		private string alsoKnownAs;
		private Image poster;


		Thread t1;
		Thread t2;

        public imdbMovieParser(uint imdbID) 
        {
            this.imdbID = imdbID;
            this.mainPageRequest = WebRequest.Create(Regex.Replace(URL, placholder, imdbID.ToString()));
			this.awardsPageRequest = WebRequest.Create(Regex.Replace(URLAwards, placholder, imdbID.ToString()));
			this.creditsPageRequest = WebRequest.Create(Regex.Replace(URLcredits, placholder, imdbID.ToString()));
			
			genres = new List<string>();
			countries = new List<string>();
			languages = new List<string>();
			directors = new List<uint>();
			writers = new List<uint>();
			roles = new List<Tuple<uint, string>>();
        }

        public void doRequest()
        {
			t1 = new Thread(new ThreadStart(t1Start));
			t2 = new Thread(new ThreadStart(t2Start));
			t1.Start();
			t2.Start();
        }

		private void t1Start()
		{
			getMainPage();
			getPicture();
		}
		private void t2Start()
		{
			getAwardsPage();
			getCreditsPage();
			t1.Join();
			doParse();
		}

		private void getMainPage()
		{
			WebResponse resp = mainPageRequest.GetResponse();
			StreamReader r = new StreamReader(resp.GetResponseStream());
			mainPage = r.ReadToEnd();
		}


		private void getAwardsPage()
		{
			WebResponse resp = awardsPageRequest.GetResponse();
			StreamReader r = new StreamReader(resp.GetResponseStream());
			awardsPage = r.ReadToEnd();
		}

		private void getCreditsPage()
		{
			WebResponse resp = creditsPageRequest.GetResponse();
			StreamReader r = new StreamReader(resp.GetResponseStream());
			creditsPage = r.ReadToEnd();
		}

		private void getPicture()
		{
			Match m = Regex.Match(mainPage, mediaURLRegex);
			string pictureURL = m.Groups["url"].Value+ ".jpg";
			WebResponse resp  = WebRequest.Create(pictureURL).GetResponse();
			poster = Image.FromStream(resp.GetResponseStream(), true, true);
			poster.Save(this.imdbID.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
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
			
			printResults();
		}


        private void extractTitleAndYear()
        {
            Match m = Regex.Match(mainPage, titleAndYearRegex);
            this.title = m.Groups[1].Value.Trim();
            year = int.Parse(m.Groups[2].Value);

        }
        private void extractPlot()
        {
            Match m = Regex.Match(mainPage, plotRegex);
            this.plot = m.Groups["plot"].Value;
            this.plot = Regex.Replace(plot, writtenByRegex, "").Trim(); // Remove all html tags
        }
        private void extractRuntime()
        {
            Match m = Regex.Match(mainPage, runtimeRegex);
            this.runtime = int.Parse( m.Groups["time"].Value);

        }
        private void extractOriginalTitle()
        {
            Match m = Regex.Match(mainPage, originalTitleRegex);
            this.originalTitle = m.Groups["orgTitle"].Value.Trim();
        }
		private void extractGenres()
		{
			Match m = Regex.Match(mainPage, genresRegex);
			string genreString = m.Groups["genres"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				genres.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractIMDBRating()
		{
			Match m = Regex.Match(mainPage, imdbRatingRegex);
			string tmpString = Regex.Replace(m.Groups["rating"].Value, @"\D", "");
			imdbRating = int.Parse(tmpString);
			
		}
		private void extractIMDBRatingVotes()
		{
			Match m = Regex.Match(mainPage, imdbRatingVotesRegex);
			string tmpString = Regex.Replace(m.Groups["votes"].Value, @"\D", "");
			imdbRatingVotes = int.Parse(tmpString);
		}
		private void extractCountries() 
		{
			Match m = Regex.Match(mainPage, countriesRegex);
			string genreString = m.Groups["country"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				countries.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractLanguages()
		{
			Match m = Regex.Match(mainPage, languagesRegex);
			string genreString = m.Groups["lang"].Value;
			MatchCollection mc = Regex.Matches(genreString, getLinkTextRegex);
			foreach (Match match in mc)
			{
				languages.Add(match.Groups["g"].Value.Trim());
			}
		}
		private void extractAlsoKnownAs()
		{
			Match m = Regex.Match(mainPage, alsoKnownAsRegex);
			this.alsoKnownAs = m.Groups["ana"].Value.Trim();
		}
		private void extractDirectors() 
		{
			Match m = Regex.Match(creditsPage, directorsRegex);
			string tmp = m.Groups["directors"].Value;
			
			MatchCollection mc = Regex.Matches(tmp, getNameFromURLRegex);
			foreach (Match match in mc)
			{
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				directors.Add(uint.Parse(id));
			} 

		}
		private void extractWriters()
		{
			Match m = Regex.Match(creditsPage, writersRegex);
			string tmp = m.Groups["writers"].Value;
			MatchCollection mc = Regex.Matches(tmp, getNameFromURLRegex);
			foreach (Match match in mc)
			{
				string id = Regex.Replace(match.Groups["g"].Value.Trim(), @"\D", "");
				writers.Add(uint.Parse(id));
			} 
		}
		private void extractCast()
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
				roles.Add(Tuple.Create<uint, string>(nm, role));
			} 
		}


        //Methode um sich bei IMDB einzuloggen um so die seiten zu laden, die angezeigt werden wenn man eingeloggt ist, vorallem, liste aller bewerteten Filme!
        CookieContainer cookie;
        public void Login(string user, string password)
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://secure.imdb.com/register-imdb/login");

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string login = string.Format("login={0}&password={1}", user, password);
            byte[] postbuf = Encoding.ASCII.GetBytes(login);
            req.ContentLength = postbuf.Length;
            Stream rs = req.GetRequestStream();
            rs.Write(postbuf, 0, postbuf.Length);
            rs.Close();

            cookie = req.CookieContainer = new CookieContainer();

            WebResponse resp = req.GetResponse();
            resp.Close();
            GetPage("http://www.imdb.com/title/tt0276830/");
        }

        public void GetPage(string path)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(path);
            req.CookieContainer = cookie;
            WebResponse resp = req.GetResponse();
            string t = new StreamReader(resp.GetResponseStream(), Encoding.Default).ReadToEnd();
            Console.Write(t);
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
			Console.WriteLine("Votes: " + this.imdbRatingVotes);
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
			Console.Write("Directors: ");
			foreach (uint s in directors)
			{
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("");
			Console.Write("Writers: ");
			foreach (uint s in writers)
			{
				Console.Write(s.ToString() + ", ");
			}
			Console.WriteLine("");

			Console.Write("Cast: ");
			foreach (Tuple<uint,string> t in roles)
			{
				Console.WriteLine(t.Item1.ToString() + " as " + t.Item2);
			}
		}

    }

}
