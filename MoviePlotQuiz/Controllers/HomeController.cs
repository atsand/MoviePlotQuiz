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

//Home controller - handles the logic for preparing and tracking the plot quiz
namespace MoviePlotQuiz.Controllers
{
    public class HomeController : Controller
    {
        //create a quiz object, that gets reset with every new quiz at QuizStart()       
        public static Quiz quiz = new Quiz();
        //static list that can be used without declaring an object. Contains movie objects
        public static List<Movie> movieList = new List<Movie>();
        //creates a leader object, that stores the data needed to add the player to the leaderboards
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

        //starts a new quiz, taking parameters from the QuizOptions View page. Sets the quiz properties
        //based on the options selected by the player.
         
        public ActionResult QuizStart(Options options)
        {
            quiz = new Quiz();
            quiz.Genre = options.Genre;
            quiz.Difficulty = options.Difficulty;
            quiz.QuestionCount = options.QuestionCount;
            List<string> titleList = new List<string>();
            
            //gets incorrect answer options and stores them in the list, used to generate radio button
            //options.
            quiz.fillerList = IDs1Controller.FillerTitleList(quiz);

            //gets enough correct answers to fill out a quiz, based on user selection.
            for (int i = 0; i < quiz.QuestionCount; i++)
            {
                titleList.Add(IDs1Controller.RandomId(quiz));
            }

            //for each movie in the list of answers/titles, get the movie's info from the API
            GetMovieData(titleList);

            //goes to the beginning of the quiz.
            return RedirectToAction("QuizPage");
        }

        //for each movie in the list of answers/titles, get the movie's info from the API

        public void GetMovieData(List<string> idList)
        {
            quiz.movieList = new List<Movie>();

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
                String data = rd.ReadToEnd();

                //parses the streamreaders data string into a JObject, with key/value pairs
                JObject movieJObject = JObject.Parse(data);

                //changes the Jobject into a movie object
                Movie movie = movieJObject.ToObject<Movie>();

                //adds the movie object to a list so it can be iterated later.
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
           
            //gets the movie at index [QuestionNum], of the movieList, and adds it to the option list
            options.Add(quiz.movieList[quiz.QuestionNum].Title);

            //rng object to generate random numbers for selecting titles.
            Random rng = new Random();

            //while we have less options than the quiz calls for, based on difficulty, 
            //add a filler option, for the sessions that print out on the quiz page
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


        //from the options list, set a session called titleI , to populate radio buttons on quiz page
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

        //increments the question number, and goes to the summary page after all questions are answered
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

        //logs the players guess, and increments number right or wrong accordingly, then goes back to quiz
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

        //once all questions are answered, calc % correct, then go to the summary page to show results
        public ActionResult Summary()
        {
            quiz.SetPercent();
            return View(quiz);
        }

        //shows the page for setting up the quiz, player chooses difficult/number of questions
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