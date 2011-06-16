using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMovieRack.Controller.Moviefillout {
	public class ImdbIdsImporter {

		private string file;
		private const string findIdsRegex = @"(?<id>\d+)";

		public ImdbIdsImporter(string file) {
			this.file = file;
		}

		internal void import() {
			MatchCollection mc = Regex.Matches(file, findIdsRegex);
			foreach (Match m in mc) {
				string idString = m.Groups["id"].Value;
				uint id = uint.Parse(idString);
				MovieFillOut f = new MovieFillOut(id);
				f.startFillout();
			}
		}
	}
}
