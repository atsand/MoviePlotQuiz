using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviePlotQuiz.Models
{
    public class Guess
    {
        //this holds the answer that the player guessed, so it can be compared to the correct answer
        //and the players score can be adjusted correctly.
        public string Answer { get; set; }
    }
}