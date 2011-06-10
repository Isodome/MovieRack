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
        public DateTime Birthday;
        public DateTime Deathday;
        public bool male;
        public int CountryofBirth;
        public string CityofBirth;
        public UInt64 lifetimeGross;
        public int boxofficeAverage;
        public int OscarNominations;
        public int OscarWins;
        public int OtherNominations;
        public int OtherWins;
        public int imdbID;

        public GUIPerson(int dbID, string Name, string OriginalName, string Biography, int CountryofBirth, string CityofBirth, UInt64 lifetimeGross, int boxofficeAverage, int OscarNominations, int OscarWins, int OtherNominations, int OtherWins, int imdbID)
        {
            this.dbID = dbID;
            this.Name = Name;
            this.OriginalName = OriginalName;
            this.Biography = Biography;
            this.Birthday = Birthday;
            this.Deathday = Deathday;
            this.male = male;
            this.CountryofBirth = CountryofBirth;
            this.CityofBirth = CityofBirth;
            this.lifetimeGross = lifetimeGross;
            this.boxofficeAverage = boxofficeAverage;
            this.OscarNominations = OscarNominations;
            this.OscarWins = OscarNominations;
            this.OscarNominations = OtherNominations;
            this.OtherWins = OtherWins;
            this.imdbID = imdbID;
        }
    }
}
