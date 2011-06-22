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
	public class SerialIMDBNameParser : SerialThreadJobMaster
	{

		private const string pictureRegex = @"(?<url>img_primary(.|\n|\r)*?</td>)";
		private const string pictureRegex2 = @"img_primary""\srowspan=""2"">(.|\n|\r)*?<img src=""(?<url>.*?V1).*?""";

		private string mainPage;
		public ImdbPerson person;


		public SerialIMDBNameParser(uint imdbID) {
			person = new ImdbPerson(imdbID);
		}

		public override bool run() {
			JobWebPageDownload mainPageJob = new JobWebPageDownload(IMDBUtil.getURLToName(person.imdbID));
			mainPageJob.run();
			this.mainPage = mainPageJob.getResult();
			if (mainPage == null) {
				return false;
			}
			JobLoadImage pictureJob = getPictureLoadJob();
			if (pictureJob != null) {
				pictureJob.run();
				person.image = pictureJob.getResult();
			}

			JobIMDBNameParser parseJob = new JobIMDBNameParser(mainPage, person);
			parseJob.run();
			return true;
		}

		private JobLoadImage getPictureLoadJob() {
			if (mainPage == null) {
				Console.WriteLine("mainpage ist null");
			}

			Match m =Regex.Match(mainPage, pictureRegex);
			string tmp = m.Groups["url"].Value;
			Match m2 = Regex.Match(tmp, pictureRegex2);
			
			if (m2.Success) {
				return new JobLoadImage(m2.Groups["url"].Value + ".jpg", null);
			}
			return null;
		}

	
	}

}
