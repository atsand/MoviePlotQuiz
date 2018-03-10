using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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