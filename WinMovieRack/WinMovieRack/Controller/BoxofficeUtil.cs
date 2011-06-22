using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Controller {
	public static class BoxofficeUtil {

		private const string url = "http://boxofficemojo.com/movies/?id={0}.htm";
		private const string urlWeekend = "http://boxofficemojo.com/movies/?page=weekend&id={0}.htm";
		private const string urlForeign = "http://boxofficemojo.com/movies/?page=intl&id={0}.htm";

		public static string getURLbyID(string id) {
			return string.Format(url, id);
		}
		public static string getWeekendpageURL(string id) {
			return string.Format(urlForeign, id);
		}
		public static string getForeignPageURL(string id) {
			return string.Format(urlForeign, id);
		}
	}
}
