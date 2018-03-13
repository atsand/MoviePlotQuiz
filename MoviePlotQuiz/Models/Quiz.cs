using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 This class was designed to hold the information about the quiz, to reference throughout 
     the web app and store player progress.

    It has been refactored to use Sessions instead of objects, so this class is not used currently
     */


namespace MoviePlotQuiz.Models
{
    public class Quiz
    {
        public int QuestionNum { get; set; }

        public double AnswersCorrect { get; set; }

        public double AnswersWrong { get; set; }

        public double Percent;

        public string Genre { get; set; }
        
        public int Difficulty { get; set; }

        public int QuestionCount { get; set; }

        public List<Movie> movieList;
        
        public void SetPercent()
        {
            this.Percent = (AnswersCorrect / QuestionNum)*100;
        }

        public List<string> fillerList;

    }
}