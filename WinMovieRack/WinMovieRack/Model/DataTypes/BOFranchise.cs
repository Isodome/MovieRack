using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model.DataTypes {
	public class BOFranchise {
		
		public BOFranchise(string name, string id, int rank) {
			this.name = name;
			this.id = id;
			this.rank = rank;
		}

		public int rank;
		public string name;
		public string id;
	}
}
