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

		public Movie movieData;

        private bool mainPageJobDone = false;
        private bool awardsPageJobDone = false;
        private bool creditsPageJobDone = false;
		private bool parseJobDone = false;
		private bool imageLoadJobDone = false;

        public imdbMovieParserMaster(uint imdbID)
        {
			movieData = new Movie(imdbID);

            this.mainPageJob = new JobWebPageDownload(Regex.Replace(URL, placholder, imdbID.ToString()));
			this.awardsPageJob = new JobWebPageDownload(Regex.Replace(URLAwards, placholder, imdbID.ToString()));
			this.creditsPageJob = new JobWebPageDownload(Regex.Replace(URLcredits, placholder, imdbID.ToString()));

            startJobs();
        }

        public void startJobs()
        {
            this.addJob(mainPageJob);
            this.addJob(awardsPageJob);
            this.addJob(creditsPageJob);
        }
		private ThreadJob getPictureLoadJob()
		{
			Match m = Regex.Match(mainPage, mediaURLRegex);
			imageURL = m.Groups["url"].Value + ".jpg";
			imageLoadJob =new JobLoadImage(imageURL, null);
			return imageLoadJob;
		}

        public override bool hasFinished(ThreadJob job) {
			if (job is JobWebPageDownload)
			{
				JobWebPageDownload res = (JobWebPageDownload)job;
				if (job == mainPageJob)
				{
					mainPageJobDone = true;
					this.mainPage = res.getResult();
					this.addJob(getPictureLoadJob());
				}
				else if (job == awardsPageJob)
				{
					awardsPageJobDone = true;
					this.awardsPage = res.getResult();
				}
				else if (job == creditsPageJob)
				{
					creditsPageJobDone = true;
					this.creditsPage = res.getResult();
				}

				if (mainPageJobDone && awardsPageJobDone && creditsPageJobDone)
				{
					parseJob = new JobParse(mainPage, creditsPage, awardsPage);
					this.addJob(parseJob);
				}
			}
			else if (job == parseJob)
			{
				parseJobDone = true;
			}
			else if (job == imageLoadJob)
			{
				this.movieData.poster = ((JobLoadImage)job).getResult();
				imageLoadJobDone = true;
			}
			return (awardsPageJobDone && creditsPageJobDone && imageLoadJobDone && mainPageJobDone && parseJobDone);
        }

		
		
       
    }
}
