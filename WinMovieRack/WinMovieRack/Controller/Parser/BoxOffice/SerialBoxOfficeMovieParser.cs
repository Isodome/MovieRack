using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Controller.ThreadManagement;
using WinMovieRack.Model;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class SerialBoxOfficeMovieParser : SerialThreadJobMaster {

		private string boxofficeId;
		private BoxofficeMovie movieData;

		private string mainPage;
		private string weekendPage;
		private string foreignPage;

		public SerialBoxOfficeMovieParser(string boxofficeId) {
			this.boxofficeId = boxofficeId;
			this.movieData = new BoxofficeMovie(boxofficeId);	
		}



		public override bool run() {

			JobWebPageDownload mainPageJob = new JobWebPageDownload(BoxofficeUtil.getURLbyID(boxofficeId));
			mainPageJob.run();
			mainPage = mainPageJob.getResult();
			if (mainPage == null) {
				return false;
			}
			JobWebPageDownload weekEndPageJob = new JobWebPageDownload(BoxofficeUtil.getWeekendpageURL(boxofficeId));
			weekEndPageJob.run();
			weekendPage = weekEndPageJob.getResult();
			if (weekendPage == null) {
				return false;
			}
			JobWebPageDownload foreignPageJob = new JobWebPageDownload(BoxofficeUtil.getForeignPageURL(boxofficeId));
			foreignPageJob.run();
			foreignPage = foreignPageJob.getResult();
			if (foreignPage == null) {
				return false;
			}
			JobBoxofficeMovieParser pjob = new JobBoxofficeMovieParser(mainPage,foreignPage, weekendPage, movieData);
			pjob.run();
			

			return true;
		}



		public BoxofficeMovie getResult() {
			return movieData;
		}
	}
}
