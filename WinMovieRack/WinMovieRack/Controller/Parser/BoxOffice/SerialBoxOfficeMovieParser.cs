using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class SerialBoxOfficeMovieParser : SerialThreadJobMaster {

		private string boxofficeId;
		private const string url = "http://boxofficemojo.com/movies/?id={0}.htm";
		private const string urlWeekend = "http://boxofficemojo.com/movies/?page=weekend&id={0}.htm";
		private const string urlForeign = "http://boxofficemojo.com/movies/?page=intl&id={0}.htm";

		private BoxofficeMovie movieData;

		private string mainPage;
		private string weekendPage;
		private string foreignPage;

		public SerialBoxOfficeMovieParser(string boxofficeId) {
			this.boxofficeId = boxofficeId;
			this.movieData = new BoxofficeMovie(boxofficeId);
			
		}



		public override bool run() {

			JobWebPageDownload mainPageJob = new JobWebPageDownload(string.Format(url, boxofficeId));
			mainPageJob.run();
			mainPage = mainPageJob.getResult();
			if (mainPage == null) {
				return false;
			}
			JobWebPageDownload weekEndPageJob = new JobWebPageDownload(string.Format(urlWeekend, boxofficeId));
			weekEndPageJob.run();
			weekendPage = weekEndPageJob.getResult();
			if (weekendPage == null) {
				return false;
			}
			JobWebPageDownload foreignPageJob = new JobWebPageDownload(string.Format(urlForeign, boxofficeId));
			foreignPageJob.run();
			foreignPage = foreignPageJob.getResult();
			if (foreignPage == null) {
				return false;
			}
			JobBoxofficeMainPageParser pjob = new JobBoxofficeMainPageParser(mainPage, movieData);
			pjob.run();
			JobBoxofficeWeekendPageParser WEjob = new JobBoxofficeWeekendPageParser(weekendPage, movieData);
			WEjob.run();
			JobBoxofficeForeignPageParser fjob = new JobBoxofficeForeignPageParser(foreignPage, movieData);
			fjob.run();
			

			return true;
		}



		public BoxofficeMovie getResult() {
			return movieData;
		}
	}
}
