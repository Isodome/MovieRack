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
    public class SerialImdbMovieParser : SerialThreadJobMaster
    {

		private string imageURL;



        // Full text of websites;
        public string mainPage;
        public string awardsPage;
        public string creditsPage;

        public const string mediaURLRegex = @"id=""img_primary""(.|\n|\r)*?<img src=""(?<url>.*?V1).*?""";

		public ImdbMovie movieData;
		public uint imdbID;


		public SerialImdbMovieParser(uint imdbID) {
			movieData = new ImdbMovie(imdbID);
			this.imdbID = imdbID;
        }

		public override bool run() {
			JobWebPageDownload mainPageJob = new JobWebPageDownload(IMDBUtil.getURLToMovie(imdbID));
			mainPageJob.run();
			this.mainPage = mainPageJob.getResult();
			if(mainPage == null) {
				return false;
			}
			JobWebPageDownload awardsPageJob = new JobWebPageDownload(IMDBUtil.getAwardsURLToMovie(imdbID));
			awardsPageJob.run();
			this.awardsPage = awardsPageJob.getResult();
			if(awardsPage == null) {
				return false;
			}
			JobWebPageDownload creditsPageJob = new JobWebPageDownload(IMDBUtil.getFullcreditsURLToMovie(imdbID));
			creditsPageJob.run();
			this.creditsPage = creditsPageJob.getResult();
			if(creditsPage == null) {
				return false;
			}

			JobLoadImage imageJob = getPictureLoadJob();
			imageJob.run();
			this.movieData.poster = imageJob.getResult();

			JobImdbMovieParser parseJob = new JobImdbMovieParser(mainPage, creditsPage, awardsPage, movieData);
			parseJob.run();

			return true;
		}

		private JobLoadImage getPictureLoadJob() {
			Match m = Regex.Match(mainPage, mediaURLRegex);
			imageURL = m.Groups["url"].Value + ".jpg";
			JobLoadImage imageLoadJob = new JobLoadImage(imageURL, null);
			return imageLoadJob;
		}
    }
}
