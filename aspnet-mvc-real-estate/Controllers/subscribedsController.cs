using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using aspnet_mvc_real_estate.Models;

namespace aspnet_mvc_real_estate.Controllers
{
    public class subscribedsController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        // GET: subscribeds
        public ActionResult Index()
        {
            return View(db.subscribeds.ToList());
        }

        // GET: subscribeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscribed subscribed = db.subscribeds.Find(id);
            if (subscribed == null)
            {
                return HttpNotFound();
            }
            return View(subscribed);
        }

        // GET: subscribeds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: subscribeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subscribed_id,email")] subscribed subscribed)
        {
            if (ModelState.IsValid)
            {
                db.subscribeds.Add(subscribed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscribed);
        }

        // GET: subscribeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscribed subscribed = db.subscribeds.Find(id);
            if (subscribed == null)
            {
                return HttpNotFound();
            }
            return View(subscribed);
        }

        // POST: subscribeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subscribed_id,email")] subscribed subscribed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscribed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscribed);
        }

        // GET: subscribeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subscribed subscribed = db.subscribeds.Find(id);
            if (subscribed == null)
            {
                return HttpNotFound();
            }
            return View(subscribed);
        }

        // POST: subscribeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subscribed subscribed = db.subscribeds.Find(id);
            db.subscribeds.Remove(subscribed);
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
