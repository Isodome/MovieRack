using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model.DataTypes {
	public class BOForeignInfo {
		public string country;
		public Int64 money;
		public bool isFinal;
		public BOForeignInfo(string country, Int64 money, bool isFinal) {
			this.money = money;
			this.country = country;
			this.isFinal = isFinal;

		}

		
	}
}
