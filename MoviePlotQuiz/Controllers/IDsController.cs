﻿using System;
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
    public class IDsController : Controller
    {
        private MoviesEntities db = new MoviesEntities();

        // GET: IDs
        public ActionResult Index()
        {
            return View(db.IDs.ToList());
        }

        // GET: IDs/Details/5
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

        // GET: IDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IDs/Create
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

        // GET: IDs/Edit/5
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

        // POST: IDs/Edit/5
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

        // GET: IDs/Delete/5
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

        // POST: IDs/Delete/5
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
    }
}
