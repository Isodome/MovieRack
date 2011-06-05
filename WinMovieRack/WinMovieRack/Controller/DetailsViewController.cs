using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
namespace WinMovieRack.Controller {

	public class DetailsViewController {

		private DetailsView view;

		public DetailsViewController() {

		}

		public void setDetailsView(DetailsView dv) {
			this.view = dv;
		}
	}
}
