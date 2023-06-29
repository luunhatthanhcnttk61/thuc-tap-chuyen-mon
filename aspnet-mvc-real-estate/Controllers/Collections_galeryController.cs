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
    public class Collections_galeryController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        // GET: Collections_galery
        public ActionResult Index()
        {
            var collections_galery = db.collections_galery.Include(c => c.collection);
            return View(collections_galery.ToList());
        }

        // GET: Collections_galery/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collections_galery collections_galery = db.collections_galery.Find(id);
            if (collections_galery == null)
            {
                return HttpNotFound();
            }
            return View(collections_galery);
        }

        // GET: Collections_galery/Create
        public ActionResult Create()
        {
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name");
            return View();
        }

        // POST: Collections_galery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "collections_id,collections_galery_id,img")] collections_galery collections_galery)
        {
            if (ModelState.IsValid)
            {
                db.collections_galery.Add(collections_galery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", collections_galery.collections_id);
            return View(collections_galery);
        }

        // GET: Collections_galery/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collections_galery collections_galery = db.collections_galery.Find(id);
            if (collections_galery == null)
            {
                return HttpNotFound();
            }
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", collections_galery.collections_id);
            return View(collections_galery);
        }

        // POST: Collections_galery/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "collections_id,collections_galery_id,img")] collections_galery collections_galery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collections_galery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", collections_galery.collections_id);
            return View(collections_galery);
        }

        // GET: Collections_galery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collections_galery collections_galery = db.collections_galery.Find(id);
            if (collections_galery == null)
            {
                return HttpNotFound();
            }
            return View(collections_galery);
        }

        // POST: Collections_galery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            collections_galery collections_galery = db.collections_galery.Find(id);
            db.collections_galery.Remove(collections_galery);
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
