using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMovieRack.GUI;

namespace WinMovieRack.Controller {
	public class MainWindowController {

		private Controller controller;
		private MainWindow mainWindow;

		public void setMainWindow(Controller c, MainWindow mw) {
			this.mainWindow = mw;
			this.controller = c;
		}

		public void shouldChangeView(View v) {
			controller.changeToView(v);
		}

		internal void close() {
			mainWindow.Close();
		}
	}
}
