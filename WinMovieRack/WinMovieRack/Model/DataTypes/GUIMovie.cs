using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WinMovieRack.Model
{
    public class GUIMovie
    {
        public int dbId;
        public string title;
        public string originalTitle;
        public string runtime;
        public string plot;
        public string year;
        public uint imdbID;
        public string imdbRating;
        public string imdbRatingVotes;
        public string imdbTop250;
        public string metacriticsID;
        public string metacriticsReviewRating;
        public string metacriticsUsersRating;
        public string metacriticsReviewVotes;
        public string metacriticsUserVotes;
        public string rottentomatoesID;
        public string rottenTomatoesAudience;
        public string tomatometer;
        public string rottenTomatoesAudienceVotes;
        public string tomatometerVotes;
        public string personalRating;
        public string boxofficemojoID;
        public string boxofficeWorldwide;
        public string boxofficeAmerica;
        public string boxofficeForeign;
        public string boxofficeFirstWeekend;
        public string rangFirstWeekend;
        public string rankAllTime;
        public string weeksInCinema;
        public string otherWins;
        public string otherNominations;
        public string notes;
        public string seenCount;
        public bool TVSeries;
        public DateTime lastSeen;
        public string budget;
        public double imdbRatingDouble;
        public int yearInt;

        public GUIMovie(int dbId, string title, string originalTitle, int runtime, string plot, int year, uint imdbID, int imdbRating, int imdbRatingVotes, int imdbTop250, string metacriticsID, int metacriticsReviewRating, int metacriticsUsersRating, int metacriticsReviewVotes, int metacriticsUserVotes, string rottentomatoesID, int rottenTomatoesAudience, int tomatometer, int rottenTomatoesAudienceVotes, int tomatometerVotes, int personalRating, string boxofficemojoID, UInt32 boxofficeWorldwide, UInt32 boxofficeAmerica, UInt32 boxofficeForeign, int boxofficeFirstWeekend, int rangFirstWeekend, int rankAllTime, int weeksInCinema, int otherWins, int otherNominations, string notes, bool TVSeries, int seenCount, DateTime lastSeen, UInt32 budget, SQLiteConnector db)
        {
            this.dbId = dbId;
            this.title = title;
            this.originalTitle = originalTitle;
            this.plot = plot;
            this.imdbID = imdbID;
            this.metacriticsID = metacriticsID;
            this.rottentomatoesID = rottentomatoesID;
            this.boxofficemojoID = boxofficemojoID;
            this.boxofficeWorldwide = "" + boxofficeWorldwide;
            this.boxofficeAmerica = "" + boxofficeAmerica;
            this.boxofficeForeign = "" + boxofficeForeign;
            this.notes = notes;
            this.TVSeries = TVSeries;
            this.lastSeen = lastSeen;
            this.budget = "" + budget;
            this.runtime = "" + runtime;
            this.year = "(" + year + ")";
            this.imdbRating = imdbRating / 10.0 + "/10";
            this.imdbRatingVotes = "(" + imdbRatingVotes + " Votes)";
            this.imdbTop250 = "" + imdbTop250;
            this.metacriticsReviewRating = metacriticsReviewRating + "/100";
            this.metacriticsUsersRating = "" + metacriticsUsersRating + "/10";
            this.metacriticsReviewVotes = "(" + metacriticsReviewVotes + " Votes)";
            this.metacriticsUserVotes = "(" + metacriticsUserVotes + " Votes)";
            this.rottenTomatoesAudience = rottenTomatoesAudience + "%";
            this.rottenTomatoesAudienceVotes = "(" + rottenTomatoesAudienceVotes + " Votes)";
            this.tomatometerVotes = "(" + tomatometerVotes + " Votes)";
            this.tomatometer = tomatometer + "%";
            this.personalRating = personalRating + "/100 (-/10)";
            this.boxofficeFirstWeekend = "" + boxofficeFirstWeekend;
            this.rangFirstWeekend = "" + rangFirstWeekend;
            this.rankAllTime = "" + rankAllTime;
            this.weeksInCinema = "" + weeksInCinema;
            this.otherWins = "" + otherWins;
            this.otherNominations = "" + otherNominations;
            this.imdbRatingDouble = imdbRating / 10.0;
            this.yearInt = year;
            if (this.runtime.Equals("-1"))
            {
                this.runtime = "No Runtime";
            }
            if (this.year.Equals("(-1)"))
            {
                this.year = "No Year";
            }
            if (this.imdbRating.Equals("-0,1/10"))
            {
                this.imdbRating = "No Rating";
            }
            if (this.imdbRatingVotes.Equals("(-1 Votes)"))
            {
                this.imdbRatingVotes = "(No Votes)";
            }
            if (this.personalRating.Equals("-1/100 (-/10)"))
            {
                this.personalRating = "(No Rating)";
            }

            if (this.boxofficeWorldwide.Equals("0"))
            {
                this.boxofficeWorldwide = "No Boxoffice";
            }
            if (this.boxofficeAmerica.Equals("0"))
            {
                this.boxofficeAmerica = "No Boxoffice";
            }
            if (this.boxofficeForeign.Equals("0"))
            {
                this.boxofficeForeign = "No Boxoffice";
            }
            if (this.budget.Equals("0"))
            {
                this.budget = "No Budget";
            }
            if (this.imdbTop250.Equals("-1"))
            {
                this.imdbTop250 = "No Rank";
            }
            if (this.metacriticsReviewRating.Equals("-1/100"))
            {
                this.metacriticsReviewRating = "No Rating";
            }
            if (this.metacriticsUsersRating.Equals("-1/10"))
            {
                this.metacriticsUsersRating = "No Rating";
            }
            if (this.metacriticsReviewVotes.Equals("(-1 Votes)"))
            {
                this.metacriticsReviewVotes = "(No Votes)";
            }
            if (this.metacriticsUserVotes.Equals("(-1 Votes)"))
            {
                this.metacriticsUserVotes = "(No Votes)";
            }
            if (this.rottenTomatoesAudience.Equals("-1%"))
            {
                this.rottenTomatoesAudience = "No Rating";
            }
            if (this.rottenTomatoesAudienceVotes.Equals("(-1 Votes)"))
            {
                this.rottenTomatoesAudienceVotes = "(No Votes)";
            }
            if (this.tomatometerVotes.Equals("(-1 Votes)"))
            {
                this.tomatometerVotes = "(No Votes)";
            }
            if (this.tomatometer.Equals("-1%"))
            {
                this.tomatometer = "No Rating";
            }
            if (this.boxofficeFirstWeekend.Equals("0"))
            {
                this.boxofficeFirstWeekend = "No Boxoffice";
            }
            if (this.rangFirstWeekend.Equals("-1"))
            {
                this.rangFirstWeekend = "No Rank";
            }
            if (this.rankAllTime.Equals("-1"))
            {
                this.rankAllTime = "No Rank";
            }
            if (this.weeksInCinema.Equals("-1"))
            {
                this.weeksInCinema = "No Weeks";
            }
            if (this.otherWins.Equals("-1"))
            {
                this.otherWins = "No Wins";
            }
            if (this.otherNominations.Equals("-1"))
            {
                this.otherNominations = "No Nominations";
            }
            this.seenCount = "" + db.getSeenCount(dbId);
            if (this.seenCount.Equals("-1"))
            {
                this.seenCount = "No Seen Count";
            }
       

        



        }

        public string getTitle
        {
            get { return title; }
        }
        public BitmapImage getPicture
        {
            get
            {
                BitmapImage posterBitmap = new BitmapImage();
                posterBitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                posterBitmap.BeginInit();
                posterBitmap.UriSource = new Uri(PictureHandler.getMoviePosterPath(dbId, PosterSize.LIST));
                posterBitmap.EndInit();

                return posterBitmap;
            }
        }

        public string getOriginalTitle
        {
            get { return originalTitle; }
        }
        public string getRuntime
        {
            get { return runtime; }
        }
        public string getPlot
        {
            get { return plot; }
        }
        public string getYear
        {
            get { return year; }
        }
        public string getImdbRating
        {
            get { return imdbRating; }
        }
        public string getimdbTop250
        {
            get { return imdbTop250; }
        }
        public string getSeenCount
        {
            get { return seenCount; }
        }
        public string getLastSeen
        {
            get { return lastSeen.ToShortDateString() ; }
        }
    }
}
