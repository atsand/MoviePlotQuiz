using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoviePlotQuiz.Models;


namespace MoviePlotQuiz.Controllers
{
    public class IDs1Controller : Controller
    {
        public static MoviesEntities1 db = new MoviesEntities1();
        public static List<string> used = new List<string>();
        public static Random rnd = new Random();

        // GET: IDs1
        public ActionResult Index()
        {
            
            return View(db.IDs.ToList());
           
        }



        // GET: IDs1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ID iD = db.IDs.Find(id);
            if (iD == null)
            {
                return HttpNotFound();
            }
            return View(iD);
        }

        // GET: IDs1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IDs1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID1,ImdbId,Title,Genre")] ID iD)
        {
            if (ModelState.IsValid)
            {
                db.IDs.Add(iD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iD);
        }

        // GET: IDs1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ID iD = db.IDs.Find(id);
            if (iD == null)
            {
                return HttpNotFound();
            }
            return View(iD);
        }

        // POST: IDs1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID1,ImdbId,Title,Genre")] ID iD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iD);
        }

        // GET: IDs1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ID iD = db.IDs.Find(id);
            if (iD == null)
            {
                return HttpNotFound();
            }
            return View(iD);
        }

        // POST: IDs1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ID iD = db.IDs.Find(id);
            db.IDs.Remove(iD);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Pulls random movie ID from database and compares them to previously used IDs
        public static string RandomId(string genre, int count)
        {
            int rando = rnd.Next(0, 232);
            ID[] array = db.IDs.ToArray();

            if (used.Count() == count)
            {
                used.Clear();
            }

            try
            {
                if (genre == "All")
                {
                    string randoId = array[rando].ImdbId;

                    if (used.Contains(randoId))
                    {
                        return RandomId(genre, count);
                    }
                    else
                    {
                        used.Add(randoId);
                        return randoId;
                    }
                }
                else if (array[rando].Genre.Contains(genre))
                {
                    string randoId = array[rando].ImdbId;

                    if (used.Contains(randoId))
                    {
                        return RandomId(genre, count);
                    }
                    else
                    {
                        used.Add(randoId);
                        return randoId;
                    }
                }
                else
                {
                    return RandomId(genre, count);
                }
            }
            catch (Exception)
            {
                return RandomId(genre, count);
            }
        }

        //Returns list of filler movie titles matching chosen genre
        public static List<string> FillerTitleList(string genre)
        {
            //Random rnd = new Random();
            //int Rando = rnd.Next(0, 232);
            //ID[] array = db.IDs.ToArray();
            List<string> fillerTitles = new List<string>();

            try
            {
                foreach (ID id in db.IDs)
                {
                    if (genre == "All")
                    {
                        fillerTitles.Add(id.Title);
                    }
                    else 
                    {
                        if (id.Genre.Contains(genre))
                        {
                            fillerTitles.Add(id.Title);
                        }
                    }
                }
                return fillerTitles;
                //if (array[Rando].Genre.Contains(quiz.Genre))
                //{
                //    string randomTitle = array[Rando].Title;
                //    return randomTitle;
                //}
                //else
                //{
                //    return RandomTitle(quiz);
                //}
            }
            catch (Exception)
            {
                return FillerTitleList(genre);
            }
        }
    }
}
