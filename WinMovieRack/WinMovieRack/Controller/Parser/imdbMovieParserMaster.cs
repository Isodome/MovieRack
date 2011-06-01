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
using WinMovieRack.Controller;
using WinMovieRack.Controller.Parser;


namespace WinMovieRack.Controller
{
    class imdbMovieParserMaster : ThreadJobMaster
    {
        public const string placholder = "{ID}";
        public const string URL = "http://www.imdb.com/title/tt" + placholder + "/";
        public const string URLAwards = URL + "awards";
        public const string URLcredits = URL + "fullcredits";


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


        public WebRequest mainPageRequest;
        public WebRequest awardsPageRequest;
        public WebRequest creditsPageRequest;

        // Full text of websites;
        public string mainPage;
        public string awardsPage;
        public string creditsPage;

        public const string mediaURLRegex = @"id=""img_primary""(.|\n|\r)*?<img src=""(?<url>.*?V1).*?""";

        public uint imdbID;
        public string title;
        public int year;
        public int runtime;
        public string originalTitle;
        public List<string> genres;
        public string plot;
        public int imdbRating;
        public int imdbRatingVotes;
        public List<string> countries;
        public List<string> languages;
        public List<uint> directors;
        public List<uint> writers;
        public List<Tuple<uint, string>> roles;
        public string alsoKnownAs;
        public Image poster;

        private bool mainPageLoaded = false;
        private bool awardsPageLoaded = false;
        private bool creditsPageLoaded = false;

        public imdbMovieParserMaster(uint imdbID)
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

            startJobs();
        }

        public void startJobs()
        {
            this.addJob(new JobLoadMainPage(this));
            this.addJob(new JobLoadAwardsPage(this));
            this.addJob(new JobLoadCreditsPage(this));
        }

        public override void hasFinished(ThreadJob job) {
            if (job is JobLoadMainPage)
            {
                mainPageLoaded = true;
            }
            else if (job is JobLoadAwardsPage)
            {
                awardsPageLoaded = true;
            }
            else if (job is JobLoadCreditsPage)
            {
                creditsPageLoaded = true;
            }
            else if (job is JobParse)
            {
                addJob(new JobPrintResults(this));
            }
            else if (job is JobLoadImage)
            {
                addJob(new JobParse(this));
            }


            if (mainPageLoaded && awardsPageLoaded && creditsPageLoaded)
            {
                addJob(new JobLoadImage(this));
            }
        }
       
    }
}
