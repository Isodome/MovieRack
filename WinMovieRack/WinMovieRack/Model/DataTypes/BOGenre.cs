using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model.DataTypes {
	public class BOGenre {

		public BOGenre(string name, string id, int rank) {
			this.rank = rank;
			this.name = name;
			this.id = id;
		}

		public int rank;
		public string name;
		public string id;
	}
}
