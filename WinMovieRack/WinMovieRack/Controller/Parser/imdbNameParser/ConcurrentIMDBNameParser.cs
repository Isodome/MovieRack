using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing;

namespace WinMovieRack.Controller.Parser.imdbNameParser
{
	class ConcurrentIMDBNameParser : ThreadJobMaster
	{
		private const string placeholder = "{ID}";
		private const string url = "http://www.imdb.com/name/nm" + placeholder + "/";
		private const string pictureRegex = @"<td id=""img_primary"" rowspan=""2"">(.|\n|\r)*?<img src=""(?<url>.*?V1).*?""";

		private ThreadJob mainPageJob;
		private bool mainPageJobDone = false;
		private ThreadJob pictureLoadJob;
		private bool pictureLoadJobDone = false;
		private ThreadJob parseJob;
		private bool parseJobDone = false;

		private string mainPage;


		public uint imdbID;
		public string name;
		public DateTime birthday;
		public string birthname;
		public Bitmap image;
		public ConcurrentIMDBNameParser(uint imdbID)
		{
			this.imdbID = imdbID;
			mainPageJob = new JobWebPageDownload(Regex.Replace(url, placeholder, imdbID.ToString()));
			this.addJob(mainPageJob);
		}

		private JobLoadImage getPictureLoadJob()
		{
			Match m = Regex.Match(mainPage, pictureRegex);
			string imageURL = m.Groups["url"].Value + ".jpg";
			return new JobLoadImage(imageURL, null);
		}

		public override bool hasFinished(ThreadJob job)
		{
			
			
			if (job == mainPageJob)
			{
				JobWebPageDownload res = (JobWebPageDownload)job;
				this.mainPage = res.getResult();
				mainPageJobDone = true;

				pictureLoadJob = getPictureLoadJob();
				addJob(pictureLoadJob);

				parseJob = new JobIMDBNameParser(this, mainPage);
				addJob(parseJob);
			} 
			else  if (job == parseJob)
			{
				parseJobDone = true;
			}
			else if (job == pictureLoadJob)
			{
				pictureLoadJobDone = true;
				image = ((JobLoadImage)job).getResult();
			}

			return (mainPageJobDone && parseJobDone && pictureLoadJobDone);
		}
	}

}
