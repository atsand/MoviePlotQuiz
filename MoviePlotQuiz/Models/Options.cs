using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviePlotQuiz.Models
{
    /*
        This class stores the players selected options when setting up the quiz. 
        It uses this information to generate the correct amount of Session adds to 
        store and generate the quiz.
         */

    public class Options
    {
        public string Genre { get; set; }

        public int Difficulty { get; set; }

        public int QuestionCount { get; set; }
    }
}