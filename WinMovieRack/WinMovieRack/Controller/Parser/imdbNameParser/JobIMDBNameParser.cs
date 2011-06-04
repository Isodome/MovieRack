using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WinMovieRack.Model;
using WinMovieRack.Controller.ThreadManagement;

namespace WinMovieRack.Controller.Parser.imdbNameParser
{
	class JobIMDBNameParser : ThreadJob
	{
		string mainPage;
		private ImdbPerson person;

		private const string nameRegex = @"<title>(?<name>.*?)- IMDb</title>";
		private const string brithdayRegexMonthDay = @"<a href=""/search/name\?birth_monthday=(?<month>\d{2})-(?<day>\d{2})"">";
		private const string birthdayRegexYear = @"<a href=""/search/name\?birth_year=(?<year>\d{4})";
		private const string birthnameRegex = @"<a class=""canwrap"" href=""bio"">(?<birthname>.*?)</a><br />";

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

		public void run()
		{
			extractName();
			extractBirthday();
			extractBirthname();

		}

		public void extractName()
		{
			Match m = Regex.Match(mainPage, nameRegex);
			person.name = m.Groups["name"].Value.Trim();
		}

		public void extractBirthday()
		{
			try
			{
				Match m = Regex.Match(mainPage, birthdayRegexYear);
				string yearString = m.Groups["year"].Value;
				int year = int.Parse(yearString);

				m = Regex.Match(mainPage, brithdayRegexMonthDay);
				string daystring = m.Groups["day"].Value;
				int day = int.Parse(daystring);

				string monthstring = m.Groups["month"].Value;
				int month = int.Parse(monthstring);

				person.birthday = new DateTime(year, month, day);
			}
			catch (FormatException)
			{
				Console.WriteLine("No Birthday for {0}", person.name);
			}
		}

		public void extractBirthname()
		{
			Match m = Regex.Match(mainPage, birthnameRegex);
			person.birthname = m.Groups["birthname"].Value.Trim();
		}



	}
}
