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

        public void GetFillerTitles()
        {
            string title1 = Session["title"].ToString();
            string title2 = IDs1Controller.RandomTitle();
            string title3 = IDs1Controller.RandomTitle();


            if (title2 != Session["title"] && title3 != Session["title"] && title2 != title3)
            {
                List<string> options = new List<string>() { title1, title2, title3 };

                Random rnd = new Random();
                int x = rnd.Next(0, 3);
                Session.Add("title1", options[x]);

                options.RemoveAt(x);

                int y = rnd.Next(0, 2);
                Session.Add("title2", options[y]);

                options.RemoveAt(y);

                int z = rnd.Next(0, 1);
                Session.Add("title3", options[z]);

                options.RemoveAt(z);
            }
            else
            {
                GetFillerTitles();
            }

        }

        public ActionResult Quiz()
        {
            GetMovieData(IDs1Controller.RandomId());
            GetFillerTitles();

            int id = 0;
            ViewBag.id = id;
            id++;
            return View();
        }

        public ActionResult QuizClone()
        {
            int id = 1;
            ViewBag.id = id;
            return View();
        }

        public ActionResult Summary()
        {
            return View();
        }
    }
}