using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMovieRack.Controller {
	 static class IMDBUtil {


		private const string nameRegex = @"://www\.imdb\.com/name/nm(?<id>\d+)/";
		private const string titleRegex = @"://www\.imdb\.com/title/tt(?<id>\d+)/";

		public static bool isNameURL(string url) {
			Match isName = Regex.Match(url, nameRegex);
			return (isName.Success);
		}

		public static bool isMovieUrl(string url) {
			Match isTitle = Regex.Match(url, titleRegex);
			return (isTitle.Success);
		}

		public static uint getNameIdFromUrl(string url) {
			Match isName = Regex.Match(url, nameRegex);
			uint id = uint.Parse(isName.Groups["id"].Value.Trim());
			return id;
		}
		public static uint getTitleIdFromUrl(string url) {
			Match isTitle = Regex.Match(url, titleRegex);
			uint id = uint.Parse(isTitle.Groups["id"].Value.Trim());
			return id;
		}


		public static string getURLToName(uint imdbID) {
			return string.Format("http:/http://www.imdb.com/name/nm{0}/", imdbID);
		}

		public static string getURLToMovie(uint imdbID) {
			return string.Format("http://www.imdb.com/title/tt{0}/", imdbID);
		}
	}
}
