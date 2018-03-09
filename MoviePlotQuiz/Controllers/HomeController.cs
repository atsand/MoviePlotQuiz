﻿using Newtonsoft.Json.Linq;
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

namespace MoviePlotQuiz.Controllers
{
    public class HomeController : Controller
    {
        public static Quiz quiz = new Quiz();

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

        public ActionResult QuizStart()
        {
            quiz = new Quiz();
            return RedirectToAction("QuizPage");
        }

        public void GetMovieData(string id)
        {
            string key = WebConfigurationManager.AppSettings["MovieAPIKey"];

            HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://www.omdbapi.com/?apikey=" + key + "&i=" + id));

            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());

            String data = rd.ReadToEnd();

            JObject movie = JObject.Parse(data);
           
            Session.Add("title", movie["Title"]);
            Session.Add("released", movie["Released"]);
            Session.Add("actors", movie["Actors"]);
            Session.Add("plot", movie["Plot"]);
            Session.Add("director", movie["Director"]);           
            Session.Add("poster", movie["Poster"]);
        }

        //fills the buttons with random uniqe titles 
        public void GetFillerTitles()
        {
            List<string> options = new List<string>();
            options.Add(Session["title"].ToString());

            for (int i = 0; i < (3 - 1); i++)
            {
                options.Add(IDs1Controller.RandomTitle());
            }

            if (options.Count() != options.Distinct().Count())
            {
                GetFillerTitles();
            }
            else
            {
                Random rnd = new Random();

                for (int i = 0; i < 3; i++)
                {
                    int x = rnd.Next(0, options.Count());

                    Session.Add("title" + (i + 1), options[x]);

                    options.RemoveAt(x);
                }
            }
        }

        public ActionResult QuizPage()
        {
            GetMovieData(IDs1Controller.RandomId());
            GetFillerTitles();
           
            if (quiz.QuestionNum<10)
            {
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

            if (g.Answer==Session["title"].ToString())
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
    }
}