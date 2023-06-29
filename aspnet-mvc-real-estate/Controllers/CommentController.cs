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
    public class CommentController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        // GET: Comment
        public ActionResult Index()
        {
            var news_comment = db.news_comment.Include(n => n.news);
            return View(news_comment.ToList());
        }

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news_comment news_comment = db.news_comment.Find(id);
            if (news_comment == null)
            {
                return HttpNotFound();
            }
            return View(news_comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            ViewBag.news_id = new SelectList(db.news, "news_id", "news_name");
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "news_comment_id,news_id,username,email,message")] news_comment news_comment)
        {
            if (ModelState.IsValid)
            {
                db.news_comment.Add(news_comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.news_id = new SelectList(db.news, "news_id", "news_name", news_comment.news_id);
            return View(news_comment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news_comment news_comment = db.news_comment.Find(id);
            if (news_comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.news_id = new SelectList(db.news, "news_id", "news_name", news_comment.news_id);
            return View(news_comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "news_comment_id,news_id,username,email,message")] news_comment news_comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news_comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.news_id = new SelectList(db.news, "news_id", "news_name", news_comment.news_id);
            return View(news_comment);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news_comment news_comment = db.news_comment.Find(id);
            if (news_comment == null)
            {
                return HttpNotFound();
            }
            return View(news_comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            news_comment news_comment = db.news_comment.Find(id);
            db.news_comment.Remove(news_comment);
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
