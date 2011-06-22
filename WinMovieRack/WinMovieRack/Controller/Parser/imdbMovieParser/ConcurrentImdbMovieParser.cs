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
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Controller.Parser.imdbNameParser;
using WinMovieRack.Model;

namespace WinMovieRack.Controller.Parser.imdbMovieParser
{
    public class ConcurrentImdbMovieParser : ConcThreadJobMaster
    {
        public const string placholder = "{ID}";
        public const string URL = "http://www.imdb.com/title/tt" + placholder + "/";
        public const string URLAwards = URL + "awards";
        public const string URLcredits = URL + "fullcredits";
		private string imageURL;

        public ThreadJob mainPageJob;
        public ThreadJob awardsPageJob;
        public ThreadJob creditsPageJob;
		public ThreadJob imageLoadJob;
		public ThreadJob parseJob;

        // Full text of websites;
        public string mainPage;
        public string awardsPage;
        public string creditsPage;

        public const string mediaURLRegex = @"id=""img_primary""(.|\n|\r)*?<img src=""(?<url>.*?V1).*?""";

		public ImdbMovie movieData;

        private bool mainPageJobDone = false;
        private bool awardsPageJobDone = false;
        private bool creditsPageJobDone = false;
		private bool parseJobDone = false;
		private bool imageLoadJobDone = false;
		private bool parseStared = false;

        public ConcurrentImdbMovieParser(uint imdbID)
        {
			movieData = new ImdbMovie(imdbID);

            this.mainPageJob = new JobWebPageDownload(Regex.Replace(URL, placholder, imdbID.ToString()));
			this.awardsPageJob = new JobWebPageDownload(Regex.Replace(URLAwards, placholder, imdbID.ToString()));
			this.creditsPageJob = new JobWebPageDownload(Regex.Replace(URLcredits, placholder, imdbID.ToString()));

			this.addJob(mainPageJob);
			this.addJob(awardsPageJob);
			this.addJob(creditsPageJob);
        }

		private ThreadJob getPictureLoadJob()
		{
			Match m = Regex.Match(mainPage, mediaURLRegex);
			imageURL = m.Groups["url"].Value + ".jpg";
			imageLoadJob = new JobLoadImage(imageURL, null);
			return imageLoadJob;
		}

        public override bool hasFinished(ThreadJob job) {
			bool result = false;
			if (job == mainPageJob) {
				JobWebPageDownload res = job as JobWebPageDownload;
				mainPageJobDone = true;
				this.mainPage = res.getResult();
				this.addJob(getPictureLoadJob());
			} else if (job == awardsPageJob) {
				JobWebPageDownload res = job as JobWebPageDownload;
				awardsPageJobDone = true;
				this.awardsPage = res.getResult();
			} else if (job == creditsPageJob) {
				JobWebPageDownload res = job as JobWebPageDownload;
				creditsPageJobDone = true;
				this.creditsPage = res.getResult();
			} else if (job == imageLoadJob) {
				this.movieData.poster = ((JobLoadImage)job).getResult();
				imageLoadJobDone = true;
			} else if (job == parseJob) {
				parseJobDone = true;
			}
			
			lock (this) {
				if (!parseStared && mainPageJobDone && awardsPageJobDone && creditsPageJobDone && imageLoadJobDone) {
					parseStared = true;
					parseJob = new JobImdbMovieParser(mainPage, creditsPage, awardsPage, movieData);
					parseJob.run();
					result = true;
				} 
			}

			return result;
        }

		
		
       
    }
}
