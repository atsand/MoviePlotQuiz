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
using System.Text;

//Home controller - handles the logic for preparing and tracking the plot quiz
namespace MoviePlotQuiz.Controllers
{
    public class HomeController : Controller
    {
        public Options options = new Options();
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

        //shows the page for setting up the quiz, player chooses difficult/number of questions
        public ActionResult QuizOptions()
        {
            Session.Abandon();
            return View();
        }

        //starts a new quiz, taking parameters from the QuizOptions View page. Sets the quiz properties
        //based on the options selected by the player.         
        public ActionResult QuizStart(Options options)
        {
            Session["QustionNumber"] = 0;
            Session["Genre"] = options.Genre;
            Session["Difficulty"] = options.Difficulty;
            Session["QuestionCount"] = options.QuestionCount;
            Session["AnswersWrong"] = 0;
            Session["AnswersCorrect"] = 0;
            Session["Hints"] = 3;

            List<string> movieList = new List<string>();
            List<string> fillerTitles = new List<string>();
            fillerTitles = IDs1Controller.FillerTitleList(Session["Genre"].ToString());
            Session["FillerTitles"] = fillerTitles;

            for (int i = 0; i < Convert.ToInt32(Session["QuestionCount"]); i++)
            {
                movieList.Add(IDs1Controller.RandomId(Session["Genre"].ToString(), Convert.ToInt32(Session["QuestionCount"])));
            }

            GetMovieData(movieList);

            //goes to the beginning of the quiz.
            return RedirectToAction("QuizPage");
        }

        //for each movie in the list of answers/titles, get the movie's info from the API
        public void GetMovieData(List<string> idList)
        {
            List<Movie> movieList = new List<Movie>();

            //StringBuilder sb = new StringBuilder();
            foreach (string id in idList)
            {
                //user specific key, for requesting info from the API
                string key = WebConfigurationManager.AppSettings["MovieAPIKey"];

                //builds a url to make a request from the API 
                HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://www.omdbapi.com/?apikey=" + key + "&i=" + id));

                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

                //API 's response to the request that was made.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //reads the data in from the API response
                StreamReader rd = new StreamReader(response.GetResponseStream());

                //converts the streamreader's data into a useable string
                //sb.Append(rd.ReadToEnd());
                String data = rd.ReadToEnd();

                //parses the streamreaders data string into a JObject, with key/value pairs
                JObject movieJObject = JObject.Parse(data);

                //changes the Jobject into a movie object
                Movie movie = movieJObject.ToObject<Movie>();

                movieList.Add(movie);
            }
            Session["MovieList"] = movieList;
        }

        //increments the question number, and goes to the summary page after all questions are answered
        public ActionResult QuizPage()
        {
            List<string> fillerTitles = Session["FillerTitles"] as List<string>;

            if (Convert.ToInt32(Session["QuestionNumber"]) < Convert.ToInt32(Session["QuestionCount"]))
            {
                SetQuestionSessions(GetFillerTitles(fillerTitles));
                Session["QuestionNumber"] = Convert.ToInt32(Session["QuestionNumber"]) + 1;
                return View();
            }
            else
            {
                return RedirectToAction("Summary");
            }
        }

        //fills the buttons with random unique titles 
        //need to make a list of all movies that match the genre picked(not just for one question)
        public List<string> GetFillerTitles(List<string> fillerTitles)
        {
            List<Movie> movieList = Session["MovieList"] as List<Movie>;
            List<string> options = new List<string>();
            options.Add(movieList[Convert.ToInt32(Session["QuestionNumber"])].Title.ToString());

            //rng object to generate random numbers for selecting titles.
            Random rng = new Random();

            while (options.Count() < Convert.ToInt32(Session["Difficulty"]))
            {
                int random = rng.Next(0, fillerTitles.Count());

                if (!options.Contains(fillerTitles[random].ToString()))
                {
                    options.Add(fillerTitles[random]);
                }
            }

            return options;
        }

        public void SetQuestionSessions(List<string> options)
        {
            Random rnd = new Random();

            for (int i = 0; i < Convert.ToInt32(Session["Difficulty"]); i++)
            {
                int x = rnd.Next(0, options.Count());

                Session.Add("title" + (i + 1), options[x]);

                options.RemoveAt(x);
            }
        }

        //logs the players guess, and increments number right or wrong accordingly, then goes back to quiz
        public ActionResult QuizClone(Guess g)
        {
            Session.Add("UserAnswer", g.Answer.ToString());
            //cast Session["MovieList"] as a list of Movie objects
            List<Movie> movieList = Session["MovieList"] as List<Movie>;

            if (g.Answer== movieList[Convert.ToInt32(Session["QuestionNumber"]) - 1].Title)
            {
                Session["AnswersCorrect"] = Convert.ToInt32(Session["AnswersCorrect"]) + 1;
            }
            else
            {
                Session["AnswersWrong"] = Convert.ToInt32(Session["AnswersWrong"]) + 1;
            }

            return View();
        }

        //once all questions are answered, calc % correct, then go to the summary page to show results
        public ActionResult Summary()
        {
            double right = Convert.ToDouble(Session["AnswersCorrect"]);
            double total = Convert.ToDouble(Session["QuestionCount"]);
            double percent = (right / total) * 100;
            Session["Percent"] = percent;

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
                //ctrl+click on Leaderboard to see the Model. Inside Model1.edmx/Model1.tt/Leaderboard.cs
                leader = new Leaderboard();

                leader.Name = Player.Name;
                if ((int)Session["Difficulty"] == 3)
                {
                    leader.Difficulty = "Easy";
                }
                else if ((int)Session["Difficulty"] == 5)
                {
                    leader.Difficulty = "Medium";
                }
                else if ((int)Session["Difficulty"] == 10)
                {
                    leader.Difficulty = "Hard";
                }
                leader.Genre = Session["Genre"].ToString();
                leader.Questions = Convert.ToInt32(Session["QuestionCount"]);
                leader.Correct = Convert.ToInt32(Session["AnswersCorrect"]);
                leader.Percentage = Convert.ToDouble(Session["Percent"]);
                leader.Score = (Convert.ToInt32(Session["AnswersCorrect"]) * Convert.ToInt32(Session["Difficulty"]));
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
            //tries to redirect to LeaderboardController/Index, if it fails go to Home
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