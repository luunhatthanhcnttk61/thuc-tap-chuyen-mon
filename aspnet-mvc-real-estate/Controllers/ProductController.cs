using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using aspnet_mvc_real_estate.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace aspnet_mvc_real_estate.Controllers
{
    public class ProductController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        // GET: Product
        public ActionResult Index(string searchString, int? page)
        {
            if (page == null) page = 1;
            ViewBag.Keyword = searchString;
            var products = db.products.Include(c => c.collection).Include(p => p.products_galery).OrderByDescending(c => c.products_id);
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.products_name.Contains(searchString)).Include(c => c.collection).Include(p => p.products_galery).OrderByDescending(c => c.products_id);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // GET: Product/Details/5
        public ActionResult ProductItem(string slug, int id)
        {
            List<ProductViewModel> SameTypeProducts = new List<ProductViewModel>();
            string thumb;
            
            // Increase view number when see product
            var product = db.products.Where(p => p.products_id == id).FirstOrDefault();
            product.viewCount++;
            db.SaveChanges();
            var SameTypeProduct = db.products.Where(p => p.type == product.type).Where(p => p.products_id != product.products_id).Take(3).OrderByDescending(p => p.products_id).ToList();
            foreach (var item in SameTypeProduct)
            {
                if (db.products_galery.Where(p => p.products_id == item.products_id).Count() > 0)
                {
                    thumb = db.products_galery.Where(p => p.products_id == item.products_id).FirstOrDefault().img;
                }
                else
                {
                    thumb = "404.jpg";
                }
                SameTypeProducts.Add(new ProductViewModel(item, thumb));
            }
            ViewBag.SameTypeProduct = SameTypeProducts;
            return View(product);
        }

        // GET: Product/Category
        public ActionResult Category(int? collection)
        {
            List<product> products = db.products.Include(c => c.collection).Include(p => p.products_galery).OrderByDescending(p => p.products_id).ToList();
            ViewBag.HeadTitle = "Sản phẩm Bất động sản";
            if (collection != null)
            {
                products = products.Where(p => p.collections_id == collection).ToList();
                ViewBag.HeadTitle = products.FirstOrDefault().collection.collections_name;
                ViewBag.collection_id = products.FirstOrDefault().collection.collections_id;
            }
            ViewBag.typeSort = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bất động sản mới nhất (Mặc định)", Value = "1" },
                new SelectListItem { Text = "Giá giảm dần", Value = "2" },
                new SelectListItem { Text = "Giá tăng dần", Value = "3" },
                new SelectListItem { Text = "Bất động sản cũ nhất", Value = "4" }
            };
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult Category(string typeSort, int? collection)
        {
            List<product> products = db.products.Include(c => c.collection).Include(p => p.products_galery).OrderByDescending(p => p.products_id).ToList();
            if (!string.IsNullOrEmpty(typeSort))
            {
                if (typeSort == "2")
                {
                    products = products.OrderByDescending(p => p.price).ToList();
                    if (collection != null)
                    {
                        products = products.Where(p => p.collections_id == collection).ToList();
                        ViewBag.HeadTitle = products.FirstOrDefault().collection.collections_name;
                        ViewBag.collection_id = products.FirstOrDefault().collection.collections_id;
                    }
                }
                else if (typeSort == "3")
                {
                    products = products.OrderBy(p => p.price).ToList();
                    if (collection != null)
                    {
                        products = products.Where(p => p.collections_id == collection).ToList();
                        ViewBag.HeadTitle = products.FirstOrDefault().collection.collections_name;
                        ViewBag.collection_id = products.FirstOrDefault().collection.collections_id;
                    }
                }
                else if (typeSort == "4")
                {
                    products = products.OrderBy(p => p.products_id).ToList();
                    if (collection != null)
                    {
                        products = products.Where(p => p.collections_id == collection).ToList();
                        ViewBag.HeadTitle = products.FirstOrDefault().collection.collections_name;
                        ViewBag.collection_id = products.FirstOrDefault().collection.collections_id;
                    }
                }
            }


            ViewBag.typeSort = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bất động sản mới nhất (Mặc định)", Value = "1" },
                new SelectListItem { Text = "Giá giảm dần", Value = "2" },
                new SelectListItem { Text = "Giá tăng dần", Value = "3" },
                new SelectListItem { Text = "Bất động sản cũ nhất", Value = "4" }
            };
            return View(products);
        }

        public ActionResult Galery(int id)
        {
            ViewBag.id = id;
            var galery = db.products_galery.Where(i => i.products_id == id).ToList();
            return View(galery);
        }

        // GET: Collections_galery/Create
        public ActionResult addImages(int id)
        {
            return View();
        }

        // POST: Collections_galery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addImages(int id, [Bind(Include = "products_id,products_galery_id,img")] products_galery products_Galery, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                if (img != null && img.ContentLength > 0)
                {
                    var product = db.products.Where(i => i.products_id == id).Include(g => g.products_galery).FirstOrDefault();
                    string photo_id;
                    if (product.products_galery.Count() > 0)
                    {
                        photo_id = (Convert.ToInt32(product.products_galery.OrderByDescending(i => i.products_galery_id).First().products_galery_id) + 1).ToString();
                    }
                    else
                    {
                        photo_id = "";
                    }
                    var fileName = product.products_slug + "-" + photo_id + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Product"), fileName);
                    img.SaveAs(path);
                    products_Galery.img = fileName;
                }
                products_Galery.products_id = id;
                db.products_galery.Add(products_Galery);
                db.SaveChanges();
                return RedirectToAction("Galery/" + id);
            }

            ViewBag.products_id = new SelectList(db.products, "products_id", "products_name", products_Galery.products_id);
            return View(products_Galery);
        }
        // GET: Collections_galery/Delete/5
        public ActionResult DeleteImages(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            products_galery products_galery = db.products_galery.Find(id);
            if (products_galery == null)
            {
                return HttpNotFound();
            }
            return View(products_galery);
        }

        // POST: Collections_galery/Delete/5
        [HttpPost, ActionName("DeleteImages")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImagesConfirmed(int id)
        {
            products_galery products_galery = db.products_galery.Find(id);
            db.products_galery.Remove(products_galery);
            db.SaveChanges();
            return RedirectToAction("Galery/" + products_galery.products_id);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "products_id,products_name,products_slug,area,information,desciption,address,price,type,viewCount,isHot,collections_id")] product product)
        {
            if (ModelState.IsValid)
            {
                product.products_slug = GenerateHelpers.GenerateSlug(product.products_name);
                db.products.Add(product);
                db.SaveChanges();
                EmailModels email = new EmailModels();
                email.Subject = "Bất động sản mới " + product.products_name;
                email.Body = "Skylands chào bạn!<br/>Chúng tôi rất vui khi được giới thiệu với bạn về một sản phẩm mới từ công ty chúng tôi.<br/><br/>Bạn có xem chi tiết về bất động sản này: <a href=\"http://"+ Request.Url.Authority + "/" + product.products_slug +"\">Xem bài viết</a><br/>Cảm ơn!";
                email.To = "admin@luunhatthanh.com";
                List<string> cclist = db.subscribeds.Select(i => i.email).ToList();
                MessageHelpers.SendEmail(email, cclist);
                return RedirectToAction("Index");
            }

            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", product.collections_id);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", product.collections_id);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "products_id,products_name,products_slug,area,information,desciption,address,price,type,viewCount,isHot,collections_id")] product product)
        {
            if (ModelState.IsValid)
            {
                product.products_slug = GenerateHelpers.GenerateSlug(product.products_name);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.collections_id = new SelectList(db.collections, "collections_id", "collections_name", product.collections_id);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<products_galery> galeries = db.products_galery.Where(p => p.products_id == id).ToList();
            // Delete all picture of this product
            if (galeries != null)
            {
                foreach(products_galery galery in galeries)
                {
                    db.products_galery.Remove(galery);
                }
            }
            product product = db.products.Find(id);
            db.products.Remove(product);
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
