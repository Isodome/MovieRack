using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model
{
    public class GUIMovie
    {
        public int dbId;
        public string title;
        public string originalTitle;
        public int runtime;
        public string plot;
        public int year;
        public int imdbID;
        public int imdbRating;
        public int imdbRatingVotes;
        public int imdbTop250;
        public string metacriticsID;
        public int metacriticsReviewRating;
        public int metacriticsUsersRating;
        public string rottentomatoesID;
        public int rottenTomatoesAudience;
        public int tomatometer;
        public int personalRating;
        public string boxofficemojoID;
        public UInt32 boxofficeWorldwide;
        public UInt32 boxofficeAmerica;
        public UInt32 boxofficeForeign;
        public int boxofficeFirstWeekend;
        public int rangFirstWeekend;
        public int rankAllTime;
        public int weeksInCinema;
        public int otherWins;
        public int otherNominations;
        public string notes;
        public int seenCount;
        public bool TVSeries;
        public DateTime lastSeen;

        public GUIMovie(int dbId, string title, string originalTitle, int runtime, string plot, int year, int imdbID, int imdbRating, int imdbRatingVotes, int imdbTop250, string metacriticsID, int metacriticsReviewRating, int metacriticsUsersRating, string rottentomatoesID, int rottenTomatoesAudience, int tomatometer, int personalRating, string boxofficemojoID, UInt32 boxofficeWorldwide, UInt32 boxofficeAmerica, UInt32 boxofficeForeign, int boxofficeFirstWeekend, int rangFirstWeekend, int rankAllTime, int weeksInCinema, int otherWins, int otherNominations, string notes, bool TVSeries, int seenCount, DateTime lastSeen)
        {
            this.dbId = dbId;
            this.title = title;
            this.originalTitle = originalTitle;
            this.runtime = runtime;
            this.plot = plot;
            this.year = year;
            this.imdbID = imdbID;
            this.imdbRating = imdbRating;
            this.imdbRatingVotes = imdbRatingVotes;
            this.imdbTop250 = imdbTop250;
            this.metacriticsID = metacriticsID;
            this.metacriticsReviewRating = metacriticsReviewRating;
            this.metacriticsUsersRating = metacriticsUsersRating;
            this.rottentomatoesID = rottentomatoesID;
            this.rottenTomatoesAudience = rottenTomatoesAudience;
            this.tomatometer = tomatometer;
            this.personalRating = personalRating;
            this.boxofficemojoID = boxofficemojoID;
            this.boxofficeWorldwide = boxofficeWorldwide;
            this.boxofficeAmerica = boxofficeAmerica;
            this.boxofficeForeign = boxofficeForeign;
            this.boxofficeFirstWeekend = boxofficeFirstWeekend;
            this.rangFirstWeekend = rangFirstWeekend;
            this.rankAllTime = rankAllTime;
            this.weeksInCinema = weeksInCinema;
            this.otherWins = otherWins;
            this.otherNominations = otherNominations;
            this.notes = notes;
            this.seenCount = seenCount;
            //   this.TVSeries = TVSeries; // mus noch gemacht werden
            //  this.lastSeen = lastSeen;

        }
    }
}
