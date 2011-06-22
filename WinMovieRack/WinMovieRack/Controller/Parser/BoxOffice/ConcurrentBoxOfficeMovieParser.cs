using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class ConcurrentBoxOfficeMovieParser : ConcThreadJobMaster {

		private string boxofficeId;
		private const string url = "http://boxofficemojo.com/movies/?id={0}.htm";
		private const string urlWeekend = "http://boxofficemojo.com/movies/?page=weekend&id={0}.htm";
		private const string urlForeign = "http://boxofficemojo.com/movies/?page=intl&id={0}.htm";


		private JobWebPageDownload mainPageJob;
		private JobWebPageDownload weekEndPageJob;
		private JobWebPageDownload foreignPageJob;


		private BoxofficeMovie movieData;

		private string mainPage;
		private string weekendPage;
		private string foreignPage;

		public ConcurrentBoxOfficeMovieParser(string boxofficeId) {
			this.boxofficeId = boxofficeId;
			this.movieData = new BoxofficeMovie(boxofficeId);
			mainPageJob = new JobWebPageDownload(string.Format(url, boxofficeId));
			weekEndPageJob = new JobWebPageDownload(string.Format(urlWeekend, boxofficeId));
			foreignPageJob = new JobWebPageDownload(string.Format(urlForeign, boxofficeId));
			this.addJob(mainPageJob);
			this.addJob(weekEndPageJob);
			this.addJob(foreignPageJob);
		}



		public override bool hasFinished(ThreadJob job) {
			if (job == mainPageJob) {
				mainPage = (job as JobWebPageDownload).getResult();
				JobBoxofficeMainPageParser pjob = new JobBoxofficeMainPageParser(mainPage, movieData);
				pjob.run();
			} else if (job == weekEndPageJob) {
				weekendPage = (job as JobWebPageDownload).getResult();
				JobBoxofficeWeekendPageParser WEjob = new JobBoxofficeWeekendPageParser(weekendPage, movieData);
				WEjob.run();
			} else if (job == foreignPageJob) {
				foreignPage = (job as JobWebPageDownload).getResult();
				JobBoxofficeForeignPageParser fjob = new JobBoxofficeForeignPageParser(foreignPage, movieData);
				fjob.run();
			}

			return false;
		}



		public BoxofficeMovie getResult() {
			return movieData;
		}
	}
}
