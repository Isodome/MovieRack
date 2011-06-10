using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.Controller.Parser.imdbNameParser {
	public class JobIMDBNameParser : ThreadJob {
		string mainPage;
		private ImdbPerson person;

		private const string nameRegex = @"<title>(?<name>.*?)- IMDb</title>";
		private const string brithdayRegexMonthDay = @"<a href=""/search/name\?birth_monthday=(?<month>\d{2})-(?<day>\d{2})"">";
		private const string birthdayRegexYear = @"<a href=""/search/name\?birth_year=(?<year>\d{4})";
		private const string birthnameRegex = @"<a class=""canwrap"" href=""bio"">(?<birthname>.*?)</a><br />";
		private const string deathRegex = @"<h4 class=""inline"">Died:</h4>(?<died>(.|\r|\n)*?</a>(.|\r|\n)*?</a>)";
		private const string deathyearRegex = @"<a href=""/search/name\?death_date=(?<year>\d{4})";
		private const string deathMonthDayRegex = @"<a href=""/date/(?<month>\d{2})-(?<day>\d{2})/deaths""";
		private const string genderRegex = @"id=""jumpto"">(?<jobs>(.|\r|\n)*?)</div>";
		private const string getLinkTextRegex = "<a href=.*?>(?<g>.*?)</a>";

		public JobIMDBNameParser(string mainPage) {
			this.initialize(mainPage, new ImdbPerson());
		}

		public JobIMDBNameParser(string mainPage, ImdbPerson person) {
			this.initialize(mainPage, person);
		}

		private void initialize(string mainPage, ImdbPerson person) {
			this.person = person;
			this.mainPage = mainPage;
		}

		public void run() {
			extractName();
			extractBirthday();
			extractDeathday();
			extractBirthname();
			extractGender();
		}

		public void extractDeathday() {
 			Match m = Regex.Match(mainPage, deathRegex);
			string jobs = m.Groups["died"].Value;
			if (jobs.Length > 0) {
				try {
					m = Regex.Match(mainPage, deathyearRegex);
					string yearString = m.Groups["year"].Value;
					int year = int.Parse(yearString);

					m = Regex.Match(mainPage, deathMonthDayRegex);
					string daystring = m.Groups["day"].Value;
					int day = int.Parse(daystring);

					string monthstring = m.Groups["month"].Value;
					int month = int.Parse(monthstring);
					person.deathday = new DateTime(year, month, day);
				} catch (FormatException) {
					this.person.deathday = DateTime.MinValue;
				}
			}
			
		}

		public void extractGender() {
			this.person.gender = 'n';
			Match m = Regex.Match(mainPage, genderRegex);
			string jobs = m.Groups["jobs"].Value;

			if (Regex.Match(jobs, @"Actress").Success) {
				this.person.gender = 'f';
			} else if (Regex.Match(jobs, @"Actor").Success) {
				this.person.gender = 'm';
			}

		}

		public void extractName() {
			Match m = Regex.Match(mainPage, nameRegex);
			person.name = m.Groups["name"].Value.Trim();
		}

		public void extractBirthday() {
			try {
				Match m = Regex.Match(mainPage, birthdayRegexYear);
				string yearString = m.Groups["year"].Value;
				int year = int.Parse(yearString);

				m = Regex.Match(mainPage, brithdayRegexMonthDay);
				string daystring = m.Groups["day"].Value;
				int day = int.Parse(daystring);

				string monthstring = m.Groups["month"].Value;
				int month = int.Parse(monthstring);

				person.birthday = new DateTime(year, month, day);
			} catch (FormatException) {
				person.birthday = DateTime.MinValue;
			}
		}

		public void extractBirthname() {
			Match m = Regex.Match(mainPage, birthnameRegex);
			person.birthname = m.Groups["birthname"].Value.Trim();
		}

		


	}
}
