using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviePlotQuiz.Models
{
    public class Quiz
    {
        public double QuestionNum { get; set; }

        public double AnswersCorrect { get; set; }

        public double AnswersWrong { get; set; }

        public double Percent;

        public string Genre { get; set; }
        
        public int Difficulty { get; set; }

        public int QuestionCount { get; set; }

        public void SetPercent()
        {
            this.Percent = (AnswersCorrect / QuestionNum)*100;
        }

    }
}