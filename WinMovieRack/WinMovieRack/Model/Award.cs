using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model {
	public class Award {
		public bool isOscar;
		public bool won;
		public int year;
		public string category;
		public string award;
		public List<uint> persons;

		public Award() {
			persons = new List<uint>();
		}

		public override string ToString() {
			StringBuilder b = new StringBuilder();
			for (int i = 0 ; i < persons.Count ; i++) {
				if (i != 0) {
					b.Append(", ");
				}
				b.Append(persons[i].ToString());
				
			}
			return String.Format("Won: '{0}', award: '{4}', isOscar: '{1}', category: '{3}', year: '{2}', person: '{5}'", won, isOscar, year, category, award, b.ToString());
		}
	}
}
