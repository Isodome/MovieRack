using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;
namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class JobBoxofficeWeekendPageParser : ThreadJob {


		private BoxofficeMovie movie;
		private string weekendPage;

		public JobBoxofficeWeekendPageParser(string website, BoxofficeMovie movie) {
			this.movie = movie;
			this.weekendPage = website;
		}

		public void run() {

		}

	}
}
