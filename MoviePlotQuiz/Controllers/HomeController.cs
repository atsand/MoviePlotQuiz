using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoviePlotQuiz.Models;
using System.Configuration;
using System.Web.Configuration;
using MoviePlotQuiz.Controllers;

//Need to make the GetFillerTitles make a list of enough filler titles to make the quiz since it will be done once at the start.
//We need to find a way to pass one movie object and enough filler titles to the view to fill the page.
//We must then remove the movie object from the list.
//Must make sure the chosen filler titles don't match the chosen title.
namespace MoviePlotQuiz.Controllers
{
    public class HomeController : Controller
    {
        public static Quiz quiz = new Quiz();
        public static List<Movie> movieList = new List<Movie>();
        public static Leaderboard leader = new Leaderboard();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult QuizStart(Options options)
        {
            quiz = new Quiz();
            quiz.Genre = options.Genre;
            quiz.Difficulty = options.Difficulty;
            quiz.QuestionCount = options.QuestionCount;
            List<string> titleList = new List<string>();
            quiz.fillerList = IDs1Controller.FillerTitleList(quiz);

            for (int i = 0; i < quiz.QuestionCount; i++)
            {
                titleList.Add(IDs1Controller.RandomId(quiz));

            }

            GetMovieData(titleList);


            return RedirectToAction("QuizPage");
        }

        public void GetMovieData(List<string> idList)
        {
            quiz.movieList = new List<Movie>();

            foreach (string id in idList)
            {

                string key = WebConfigurationManager.AppSettings["MovieAPIKey"];

                HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://www.omdbapi.com/?apikey=" + key + "&i=" + id));

                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader rd = new StreamReader(response.GetResponseStream());

                String data = rd.ReadToEnd();

                JObject movieJObject = JObject.Parse(data);

                Movie movie = movieJObject.ToObject<Movie>();

                quiz.movieList.Add(movie);

                //Session.Add("title", movie["Title"]);
                //Session.Add("released", movie["Released"]);
                //Session.Add("actors", movie["Actors"]);
                //Session.Add("plot", movie["Plot"]);
                //Session.Add("director", movie["Director"]);
                //Session.Add("poster", movie["Poster"]);
            }
        }

        //fills the buttons with random unique titles 
        //need to make a list of all movies that match the genre picked(not just for one question)
        public List<string> GetFillerTitles(List<string> fillerOptions)
        {
            List<string> options = new List<string>();
            options.Add(quiz.movieList[quiz.QuestionNum].Title);

            Random rng = new Random();

            while (options.Count()<quiz.Difficulty)
            {
                int random = rng.Next(0, fillerOptions.Count());

                if (!options.Contains(fillerOptions[random]))
                {
                    options.Add(fillerOptions[random]);
                }
            }

            return options;
        }

        public void SetQuestionSessions(List<string> options)
        {
            
            Random rnd = new Random();

            for (int i = 0; i < quiz.Difficulty; i++)
            {
                int x = rnd.Next(0, options.Count());

                Session.Add("title" + (i + 1), options[x]);

                options.RemoveAt(x);
            }
        }

        public ActionResult QuizPage()
        {
            if (quiz.QuestionNum<quiz.QuestionCount)
            {
                SetQuestionSessions(GetFillerTitles(quiz.fillerList));
                quiz.QuestionNum++;
                return View(quiz);
            }
            else
            {
                return RedirectToAction("Summary");
            }
        }

        public ActionResult QuizClone(Guess g)
        {
            Session.Add("UserAnswer", g.Answer.ToString());

            if (g.Answer== quiz.movieList[quiz.QuestionNum - 1].Title)
            {
                quiz.AnswersCorrect++;
            }
            else
            {
                quiz.AnswersWrong++;
            }

            return View(quiz);
        }

        public ActionResult Summary()
        {
            quiz.SetPercent();
            return View(quiz);
        }

        public ActionResult QuizOptions()
        {
            return View();
        }


        /*DAVID
         * When a player completes a quiz, they are given the option to view the leaderboard or
            add their score to it by entering their name. If the name is entered, it goes to a 
            player model and stores the name. The rest of the stats are copied from the quiz model.
            
            If they do not enter a name, this method will bypass
            adding a player, and just show the current leaderboard. 
             */
        public ActionResult AddScore(LeaderboardModel Player)
        {
            if (Player.Name != null)
            {
                //ctrl+click on Leaderboard to see the Model. Cant find actual .cs file for it...
                leader = new Leaderboard();

                leader.Name = Player.Name;
                if (quiz.Difficulty == 3)
                {
                    leader.Difficulty = "Easy";
                }
                else if (quiz.Difficulty == 5)
                {
                    leader.Difficulty = "Medium";
                }
                else if (quiz.Difficulty == 10)
                {
                    leader.Difficulty = "Hard";
                }
                leader.Genre = quiz.Genre;
                leader.Questions = quiz.QuestionCount;
                leader.Correct = quiz.AnswersCorrect;
                leader.Percentage = quiz.Percent;
                leader.Score = (quiz.AnswersCorrect * quiz.Difficulty);
                try
                {
                    //DAVID
                    //tries to pass the leader object to the addscores() action result in the
                    //leaderboard controller. 
                    return RedirectToAction("AddScores", "Leaderboards", leader);
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            //DAVID  -- if no name is given by the player then this
            //tries to redirect to LeaderboardController/Display, if it fails go to Home
            try
            {
                return RedirectToAction("Index", "LeaderboardsController");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}