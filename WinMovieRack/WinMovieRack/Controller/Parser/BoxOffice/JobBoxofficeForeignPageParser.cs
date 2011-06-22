using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.Controller.Parser.BoxOffice {
	public class JobBoxofficeForeignPageParser : ThreadJob{

		private BoxofficeMovie movie;
		private string foreignPage;

		public JobBoxofficeForeignPageParser(string website, BoxofficeMovie movie) {
			this.movie = movie;
			this.foreignPage = website;
		}

		public void run() {

		}

	}
}
