using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MoviePlotQuiz.Controllers
{
    public class HomeController : Controller
    {
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

        public void GetMovieData(string id)
        {
            HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://www.omdbapi.com/?apikey=6e7b73a4&i=" + id));

            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());

            String data = rd.ReadToEnd();

            JObject movie = JObject.Parse(data);

            ViewBag.AnswerTitle = movie["Title"];
            ViewBag.AnswerReleasedDate = movie["Released"];
            ViewBag.AnswerActors = movie["Actors"];
            ViewBag.AnswerPlot = movie["Plot"];
            ViewBag.AnswerPosterURL = movie["Poster"];
            ViewBag.AnswerDirector = movie["Director"];
        }
    }
}