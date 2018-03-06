using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviePlotQuiz.Models
{
    public class MovieModel
    {
        public string imdbID { get; set; }

        public string Title { get; set; }

        public string Plot { get; set; }

        public string Director { get; set; }

        public string Actors { get; set; }

        public string Poster { get; set; }
    }
}