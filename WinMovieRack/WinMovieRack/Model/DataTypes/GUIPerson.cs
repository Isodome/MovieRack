using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model
{
    public class GUIPerson
    {
        public int dbID;
        public string Name;
        public string OriginalName;
        public string Biography;
        public string Birthday;
        public string Deathday;
        public string gender;
        public string CountryofBirth;
        public string CityofBirth;
        public string lifetimeGross;
        public string boxofficeAverage;
        public string OscarNominations;
        public string OscarWins;
        public string OtherNominations;
        public string OtherWins;
        public int imdbID;
        public string age;

        public GUIPerson(int dbID, string Name, string OriginalName, string Biography, DateTime Birthday, DateTime Deathday, char gender, int CountryofBirth, string CityofBirth, Int64 lifetimeGross, int boxofficeAverage, int OscarNominations, int OscarWins, int OtherNominations, int OtherWins, int imdbID)
        {
            this.dbID = dbID;
            this.Name = Name;
            this.OriginalName = OriginalName;
            this.Biography = Biography;
            this.Birthday = Birthday.Day + "." + Birthday.Month + "." + Birthday.Year;
            this.Deathday = "" + Deathday.Day + "." + Deathday.Month + "." + Deathday.Year;
            this.gender = "" + gender;
            this.CountryofBirth = "" + CountryofBirth;
            this.CityofBirth = CityofBirth;
            this.lifetimeGross = "" + lifetimeGross;
            this.boxofficeAverage = "" + boxofficeAverage;
            this.OscarNominations = "" + OscarNominations;
            this.OscarWins = "" + OscarWins;
            this.OtherNominations = "" + OtherNominations;
            this.OtherWins = "" + OtherWins;
            this.imdbID = imdbID;
            int years = -1;
            if (Birthday.Year != 1)
            {
                DateTime now = DateTime.Today;
                years = now.Year - Birthday.Year;
                if (Birthday > now.AddYears(-years))
                {
                    years--;
                }
                age = "" + years;
            }
            else
            {
                age = "No Age";
            }
            if (this.Birthday.Equals("01.01.0001"))
            {
                this.Birthday = "No Birthday";
            }
            if (this.Deathday.Equals("01.01.0001"))
            {
                this.Deathday = "No Deathday";
            }
            if (this.gender.Equals("f"))
            {
                this.gender = "Female";
            }
            else if (gender.Equals("m"))
            {
                this.gender = "Male";
            }
            else
            {
                this.gender = "No Gender";
            }
            if (this.lifetimeGross.Equals("0"))
            {
                this.lifetimeGross = "No Lifetime Gross";
            }
            if (this.boxofficeAverage.Equals("-1"))
            {
                this.boxofficeAverage = "No Boxoffice Average";
            }
            if (this.OscarNominations.Equals("-1"))
            {
                this.OscarNominations = "No Oscar Nminations";
            }
            if (this.OscarWins.Equals("-1"))
            {
                this.OscarWins = "No Oscar Wins";
            }
            if (this.OtherNominations.Equals("-1"))
            {
                this.OtherNominations = "No Other Nominations";
            }
            if (this.OtherWins.Equals("-1"))
            {
                this.OtherWins = "No Other Wins";
            }
        }
    }
}
