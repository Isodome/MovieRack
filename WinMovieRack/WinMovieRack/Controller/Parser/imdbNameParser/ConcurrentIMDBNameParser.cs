using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WinMovieRack.Controller.Parser.imdbNameParser
{
	class ConcurrentIMDBNameParser : ThreadJobMaster
	{
		private const string placeholder = "{ID}";
		private const string url = "http://www.imdb.com/name/nm" + placeholder + "/";

		private ThreadJob mainPageJob;
		private bool mainPageJobDone = false;
		private ThreadJob parseJob;
		private bool parseJobStarted = false;

		private string mainPage;


		public uint imdbID;
		public string name;
		public DateTime birthday;

		public ConcurrentIMDBNameParser(uint imdbID)
		{
			this.imdbID = imdbID;
			mainPageJob = new JobWebPageDownload(Regex.Replace(url, placeholder, imdbID.ToString()));
			this.addJob(mainPageJob);
		}


		public override bool hasFinished(ThreadJob job)
		{
			if (job is JobWebPageDownload)
			{
				JobWebPageDownload res = (JobWebPageDownload)job;
				if (job == mainPageJob)
				{
					this.mainPage = res.getResult();
					mainPageJobDone = true;
				}
			}

			Monitor.Enter(this);
			if (!parseJobStarted && mainPageJobDone)
			{
				parseJob = new JobIMDBNameParser(this, mainPage);
				addJob(parseJob);
				parseJobStarted = true;
			}
			Monitor.Exit(this);

			return (job == parseJob);
		}
	}

}
