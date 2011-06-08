using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinMovieRack.Model
{
    public class MovieListData
    {
        public int idMovies;
        public string title;
        public int imdbRating;
        public string editable;
        public string posterPath;

        public MovieListData(int idMovies, string title, int imdbRating, string editable, string posterPath)
        {
            this.idMovies = idMovies;
            this.title = title;
            this.imdbRating = imdbRating;
            this.editable = editable;
            this.posterPath = posterPath;
        }
    }
}
