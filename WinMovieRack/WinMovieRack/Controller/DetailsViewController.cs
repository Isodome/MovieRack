using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;
namespace WinMovieRack.Controller {

	public class DetailsViewController {

		private DetailsView view;
		private Controller controller;

		public DetailsViewController(Controller c) {
			this.controller = c;
		}

		public void setDetailsView(DetailsView dv) {
			this.view = dv;
		}
	}
}
