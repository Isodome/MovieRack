using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinMovieRack.Controller;


namespace WinMovieRack.GUI {
	class GUI {

		private WinMovieRack.Controller.Controller controller;
		private MainWindow mainWindow;

		public GUI(WinMovieRack.Controller.Controller c) {

			this.controller = c;
			mainWindow = new MainWindow();
			mainWindow.Show();
		}
	}
}
