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

namespace aspnet_mvc_real_estate.Controllers
{
    public class CollectionController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        // GET: Collection
        public ActionResult Index(string searchString, int? page)
        {
            if (page == null) page = 1;
            var collections = db.collections.Include(c => c.collections_galery).OrderByDescending(c => c.collections_id);
            ViewBag.Keyword = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                collections = collections.Where(c => c.collections_name.Contains(searchString)).Include(c => c.collections_galery).OrderByDescending(c => c.collections_id);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(collections.ToPagedList(pageNumber, pageSize));
        }

        // GET: Collection/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collection collection = db.collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        public ActionResult Category(int? page)
        {
            if (page == null)
                page = 1;
            var collections = db.collections.Include(i => i.collections_galery).OrderByDescending(i => i.collections_id);
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(collections.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CollectionItem(string slug, int id)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            var collection = db.collections.Where(i => i.collections_id == id).Include(i => i.collections_galery).FirstOrDefault();
            var product = db.products.Where(i => i.collections_id == id).Include(i => i.products_galery).Take(3).ToList();
            string thumb;
            foreach (var item in product)
            {
                if (item.products_galery.Count() > 0)
                {
                    thumb = item.products_galery.FirstOrDefault().img;
                }
                else
                {
                    thumb = "404.jpg";
                }
                products.Add(new ProductViewModel(item, thumb));
            }
            ViewBag.products = products;
            ViewBag.videoId = GetHelpers.GetIDYoutube(collection.video);
            return View(collection);
        }

        public ActionResult Galery(int id)
        {
            ViewBag.id = id;
            var galery = db.collections_galery.Where(i => i.collections_id == id).ToList();
            return View(galery);
        }

        // GET: Collections_galery/Create
        public ActionResult addImages(int id)
        {
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name");
            return View();
        }

        // POST: Collections_galery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addImages(int id, [Bind(Include = "collections_id,collections_galery_id,img")] collections_galery collections_galery, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentLength > 0)
                {
                    var collection = db.collections.Where(i => i.collections_id == id).Include(g => g.collections_galery).FirstOrDefault();
                    string photo_id;
                    if (collection.collections_galery.Count() > 0)
                    {
                        photo_id = (Convert.ToInt32(collection.collections_galery.OrderByDescending(i => i.collections_galery_id).First().collections_galery_id) + 1).ToString();
                    }
                    else
                    {
                        photo_id = "";
                    }
                    var fileName = collection.collections_slug + "-" + photo_id + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Collections"), fileName);
                    img.SaveAs(path);
                    collections_galery.img = fileName;
                }
                collections_galery.collections_id = id;
                db.collections_galery.Add(collections_galery);
                db.SaveChanges();
                return RedirectToAction("Galery/"+id);
            }

            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", collections_galery.collections_id);
            return View(collections_galery);
        }

        // GET: Collection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collection/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(collection collection, HttpPostedFileBase thumbnail)
        {
            if (ModelState.IsValid)
            {
                collection.collections_slug = GenerateHelpers.GenerateSlug(collection.collections_name);
                db.collections.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(collection);
        }

        // GET: Collection/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collection collection = db.collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collection/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "collections_id,collections_name,collections_slug,overview,video,introduction,information,location,Thumbnail")] collection collection, HttpPostedFileBase Thumbnail, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.collections_slug = GenerateHelpers.GenerateSlug(collection.collections_name);
                    if (Thumbnail != null)
                    {
                        string _FileName = Path.GetFileName(Thumbnail.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Images/Collections"), _FileName);
                        Thumbnail.SaveAs(_path);
                       // collection.Thumbnail = _FileName;
                        // Get Path of old images for deleting it
                        _path = Path.Combine(Server.MapPath("~/Images/Collections"), form["oldimage"]);
                        if (System.IO.File.Exists(_path));
                            System.IO.File.Delete(_path);
                    }
                    else
                       // collection.Thumbnail = form["oldimage"];

                    db.Entry(collection).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Không thành công!";
                }
                return RedirectToAction("Index");
            }
            return View(collection);
        }

        // GET: Collections_galery/Delete/5
        public ActionResult DeleteImages(int? id)
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
        [HttpPost, ActionName("DeleteImages")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImagesConfirmed(int id)
        {
            collections_galery collections_galery = db.collections_galery.Find(id);
            db.collections_galery.Remove(collections_galery);
            db.SaveChanges();
            return RedirectToAction("Galery/"+collections_galery.collections_id);
        }

        // GET: Collection/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collection collection = db.collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<collections_galery> galeries = db.collections_galery.Where(p => p.collections_id == id).ToList();
            // Delete all picture of this product
            if (galeries != null)
            {
                foreach (collections_galery galery in galeries)
                {
                    db.collections_galery.Remove(galery);
                }
            }
            collection collection = db.collections.Find(id);
            db.collections.Remove(collection);
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
