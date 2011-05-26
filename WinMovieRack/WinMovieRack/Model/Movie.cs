using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack
{
    public class Movie : DBItem
    {
        public String title
        {
            get
            {
                return (title);
            }
            set
            {
                this.title = value;
            }
        }

        public int imdbID
        {
            get
            {
                return (imdbID);
            }
            set
            {
                this.imdbID = value;
            }
        }

        public string metacriticsID
        {
            get
            {
                return (metacriticsID);
            }
            set
            {
                this.metacriticsID = value;
            }
        }

        public string rottentomatoesID
        {
            get
            {
                return (rottentomatoesID);
            }
            set
            {
                this.rottentomatoesID = value;
            }
        }

        public string originalTitle
        {
            get
            {
                return (originalTitle);
            }
            set
            {
                this.originalTitle = value;
            }
        }

        public short year
        {
            get
            {
                return (year);
            }
            set
            {
                this.year = value;
            }
        }

        public string ageRating
        {
            get
            {
                return (ageRating);
            }
            set
            {
                this.ageRating = value;
            }
        }

        public short runtime
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public short imdbRating
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int imdbVoteCount
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public short metacriticsRating
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public short rottentomatoesTomatometer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public short rottentomatoesAudience
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public short imdbTop250Position
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<String> country
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int internalID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<string> language
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<string> genre
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string storyLine
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<Tuple<DateTime, string>> dateSeen
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int topActors
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
