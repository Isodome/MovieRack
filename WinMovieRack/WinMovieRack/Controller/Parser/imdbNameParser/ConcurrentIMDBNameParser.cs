using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMovieRack.Controller.Parser.imdbNameParser
{
	class ConcurrentIMDBNameParser : ThreadJobMaster
	{
		private const string placeholder = "{0}";
		private const string url = "http://www.imdb.com/name/nm" + placeholder + "/";

		private ThreadJob mainPageJob;

		private string mainPage;


		public ConcurrentIMDBNameParser(uint imdbID)
		{
			this.addJob(mainPageJob = new JobWebPageDownload(Regex.Replace(url, placeholder, imdbID.ToString())));
		}


		public override bool hasFinished(ThreadJob job)
		{
			if (job is JobWebPageDownload)
			{
				JobWebPageDownload res = (JobWebPageDownload)job;
				if (job == mainPageJob)
				{
					this.mainPage = res.getResult();
				}
			}
			return false;
		}
	}

}
