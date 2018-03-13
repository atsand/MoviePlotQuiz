using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviePlotQuiz.Models
{
    /*
        This class holds the movie data that is gathered from the OMDB API that is used. It
        is used to generate lists of correct answers and wrong answers used in the quiz.
         
    */

    public class Movie
    {
        
        public string imdbID { get; set; }

        public string Title { get; set; }

        public string Plot { get; set; }

        public string Director { get; set; }

        public string Actors { get; set; }

        public string Poster { get; set; }

        public string Released { get; set; }

        public string Genre { get; set; }
    }
}