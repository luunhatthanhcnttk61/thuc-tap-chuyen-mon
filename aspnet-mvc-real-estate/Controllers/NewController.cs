using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using aspnet_mvc_real_estate.Models;
using System.IO;
using PagedList;
using System.Web.UI;
using System.Xml.Linq;

namespace aspnet_mvc_real_estate.Controllers
{
    public class NewController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        // GET: New
        public ActionResult Index(int? page)
        {
            if(page == null || page < 1)
                page = 1;
            var post = db.news.OrderBy(p => p.news_id);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(post.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Category(int? page)
        {
            var hotNews = db.news.OrderByDescending(p => p.viewCount).Take(10);
            ViewBag.hotNews = hotNews;
            if (page == null || page < 1)
                page = 1;
            var post = db.news.OrderBy(p => p.news_id);
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(post.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Post(string slug, int id)
        {
            var hotNews = db.news.Where(p => p.news_id != id).OrderByDescending(p => p.viewCount).Take(10);
            ViewBag.hotNews = hotNews;
            var news = db.news.Where(p => p.news_id == id).FirstOrDefault<news>();
            var news_comment = db.news_comment.Where(i => i.news_id == id).OrderBy(i => i.news_comment_id).ToList();
            NewsViewModels post = new NewsViewModels(news, news_comment);
            if (post != null)
            {
                post.news.viewCount++;
                db.SaveChanges();
            }
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(int news_id, string username, string email, string message)
        {
            var slug = db.news.Where(n => n.news_id == news_id).FirstOrDefault().news_slug;
            try
            {
                if (username != null && email != null && message != null)
                {
                    db.news_comment.Add(new news_comment()
                    {
                        news_id = news_id,
                        username = username,
                        email = email,
                        message = message
                    });
                    db.SaveChanges();
                    return RedirectToAction("Post", "New", new { slug = slug, id = news_id });
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin!";
                return RedirectToAction("Post", "New", new { slug = slug, id = news_id });
            }
            return RedirectToAction("Post", "New", new { slug = slug, id = news_id });
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteComment(int id)
        {
            var news_comment = db.news_comment.Find(id);
            db.news_comment.Remove(news_comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: New/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: New/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: New/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "news_id,news_name,news_slug,content,thumbnail,postdate,viewCount")] news news, HttpPostedFileBase thumbnail)
        {
            if (ModelState.IsValid)
            {
                news.news_slug = GenerateHelpers.GenerateSlug(news.news_name);
                if ( thumbnail != null && thumbnail.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(thumbnail.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Post"), fileName);
                    thumbnail.SaveAs(path);
                    news.thumbnail = fileName;
                }
                db.news.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: New/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: New/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "news_id,news_name,news_slug,content,thumbnail,postdate,viewCount")] news news, HttpPostedFileBase thumbnail, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    news.news_slug = GenerateHelpers.GenerateSlug(news.news_name);
                    if (thumbnail != null)
                    {
                        string _FileName = Path.GetFileName(thumbnail.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Post"), _FileName);
                        thumbnail.SaveAs(_path);
                        news.thumbnail = _FileName;
                        // Get Path of old images for deleting it
                        _path = Path.Combine(Server.MapPath("~/Content/Images/Post"), form["oldimage"]);
                        if (System.IO.File.Exists(_path)) ;
                        System.IO.File.Delete(_path);
                    }
                    else
                        news.thumbnail = form["oldimage"];

                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Không thành công!";
                }
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: New/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: New/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            news news = db.news.Find(id);
            db.news.Remove(news);
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
