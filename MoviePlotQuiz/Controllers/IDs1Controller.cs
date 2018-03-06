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
        private MoviesEntities1 db = new MoviesEntities1();

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

        //Pulls random movie ID from database
        public string RandomId()
        {
            Random rnd = new Random();
            int Rando = rnd.Next(0, 232);
            ID[] array = db.IDs.ToArray();

            try
            {
                string randomId = array[Rando].ImdbId;
                return randomId;               
            }
            catch (Exception)
            {
                return RandomId();
            }
        }

        //Pulls random movie Title from database
        public string RandomTitle()
        {
            Random rnd = new Random();
            int Rando = rnd.Next(0, 232);
            ID[] array = db.IDs.ToArray();

            try
            {
                string randomTitle = array[Rando].Title;
                return randomTitle;
            }
            catch (Exception)
            {
                return RandomTitle();
            }
        }
    }
}
