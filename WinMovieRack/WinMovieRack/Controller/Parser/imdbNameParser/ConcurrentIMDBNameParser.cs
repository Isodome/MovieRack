using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.Controller.Parser.imdbNameParser
{
	public class ConcurrentIMDBNameParser : ThreadJobMaster
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

		public ImdbPerson person;


		

		public ConcurrentIMDBNameParser(uint imdbID)
		{
			person = new ImdbPerson(imdbID);

			mainPageJob = new JobWebPageDownload(Regex.Replace(url, placeholder, imdbID.ToString()));
			this.addJob(mainPageJob);
		}

		private JobLoadImage getPictureLoadJob()
		{
			if (mainPage == null) {
				Console.WriteLine("mainpage ist null");
			}
			Regex r = new Regex(pictureRegex);
			Match m =r.Match(mainPage);
			if (m.Success) {
				return new JobLoadImage(m.Groups["url"].Value + ".jpg", null);
			}
			return null;
		}

		public override bool hasFinished(ThreadJob job)
		{
			
			
			if (job == mainPageJob)
			{
				JobWebPageDownload res = (JobWebPageDownload)job;
				this.mainPage = res.getResult();
				mainPageJobDone = true;

				pictureLoadJob = getPictureLoadJob();
				if (pictureLoadJob != null) {
					addJob(pictureLoadJob);
				} else {
					pictureLoadJobDone = true;
				}

				parseJob = new JobIMDBNameParser(mainPage, person);
				addJob(parseJob);
			} 
			else  if (job == parseJob)
			{
				parseJobDone = true;
			}
			else if (job == pictureLoadJob)
			{
				pictureLoadJobDone = true;
				person.image = ((JobLoadImage)job).getResult();
			}

			return (mainPageJobDone && parseJobDone && pictureLoadJobDone);
		}
	}

}
