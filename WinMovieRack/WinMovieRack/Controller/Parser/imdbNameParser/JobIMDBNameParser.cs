using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WinMovieRack.Controller.Parser.imdbNameParser
{
	class JobIMDBNameParser : ThreadJob
	{
		string mainPage;
		private ConcurrentIMDBNameParser parent;

		private const string nameRegex = @"<title>(?<name>.*?)- IMDb</title>";
		private const string brithdayRegexMonthDay = @"<a href=""/search/name\?birth_monthday=(?<month>\d{2})-(?<day>\d{2})"">";
		private const string birthdayRegexYear = @"<a href=""/search/name\?birth_year=(?<year>\d{4})";
		private const string birthnameRegex = @"<a class=""canwrap"" href=""bio"">(?<birthname>.*?)</a><br />";

		public JobIMDBNameParser(ConcurrentIMDBNameParser parent, string mainPage)
		{
			this.parent = parent;
			this.mainPage = mainPage;
		}

		public void run()
		{
			extractName();
			extractBirthday();
			extractBirthname();

			printResults();
		}

		private void extractName()
		{
			Match m = Regex.Match(mainPage, nameRegex);
			parent.name = m.Groups["name"].Value.Trim();
		}

		private void extractBirthday()
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

				parent.birthday = new DateTime(year, month, day);
			}
			catch (FormatException)
			{
				Console.WriteLine("No Birthday for {0}", parent.name);
				
			}
		}

		private void extractBirthname()
		{
			Match m = Regex.Match(mainPage, birthnameRegex);
			parent.birthname = m.Groups["birthname"].Value.Trim();
		}

		private void printResults()
		{
			Console.WriteLine("IMDB ID: " + parent.imdbID);
			Console.WriteLine("Name: " + parent.name);
			Console.WriteLine("Birthday: " + parent.birthday.ToString());
			Console.WriteLine("Birthname: " + parent.birthname);
		}

	}
}
