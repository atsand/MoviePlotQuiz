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
    public class LeaderboardsController : Controller
    {
        private LeaderboardEntities db = new LeaderboardEntities();

        // GET: Leaderboards
        public ActionResult Index()
        {
            return View(db.Leaderboards.ToList());
        }

        // GET: Leaderboards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            if (leaderboard == null)
            {
                return HttpNotFound();
            }
            return View(leaderboard);
        }

        // GET: Leaderboards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leaderboards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Difficulty,Genre,Questions,Correct,Percentage,Score")] Leaderboard leaderboard)
        {
            if (ModelState.IsValid)
            {
                db.Leaderboards.Add(leaderboard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leaderboard);
        }

        // GET: Leaderboards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            if (leaderboard == null)
            {
                return HttpNotFound();
            }
            return View(leaderboard);
        }

        // POST: Leaderboards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Difficulty,Genre,Questions,Correct,Percentage,Score")] Leaderboard leaderboard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaderboard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaderboard);
        }

        // GET: Leaderboards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            if (leaderboard == null)
            {
                return HttpNotFound();
            }
            return View(leaderboard);
        }

        // POST: Leaderboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            db.Leaderboards.Remove(leaderboard);
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
    }
}
