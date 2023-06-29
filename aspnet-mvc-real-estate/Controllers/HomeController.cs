using aspnet_mvc_real_estate.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace aspnet_mvc_real_estate.Controllers
{
    public class HomeController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();

        public ActionResult Index()
        {
            List<ProductViewModel> HotProducts = new List<ProductViewModel>();
            List<ProductViewModel> NewProducts = new List<ProductViewModel>();
            List<ProductViewModel> TrendProducts = new List<ProductViewModel>();
            string thumb;
            List<product> HotProduct = db.products.Where(p => p.isHot == true).Take(7).OrderByDescending(p => p.products_id).ToList();
            foreach (var item in HotProduct)
            {
                if (db.products_galery.Where(p => p.products_id == item.products_id).Count() > 0)
                {
                    thumb = db.products_galery.Where(p => p.products_id == item.products_id).FirstOrDefault().img;
                }
                else
                {
                    thumb = "404.jpg";
                }
                HotProducts.Add(new ProductViewModel(item, thumb));
            }
            ViewBag.HotProduct = HotProducts;
            List<product> NewProduct = db.products.OrderByDescending(p => p.products_id).Take(10).Include(p => p.products_galery).ToList();
            foreach (var item in NewProduct)
            {
                if (db.products_galery.Where(p => p.products_id == item.products_id).Count() > 0)
                {
                    thumb = db.products_galery.Where(p => p.products_id == item.products_id).FirstOrDefault().img;
                }
                else
                {
                    thumb = "404.jpg";
                }
                NewProducts.Add(new ProductViewModel(item, thumb));
            }
            ViewBag.NewProduct = NewProducts;
            List<product> TrendProduct = db.products.OrderByDescending(p => p.viewCount).OrderByDescending(p => p.products_id).Take(10).Include(p => p.products_galery).ToList();
            foreach (var item in TrendProduct)
            {
                if (db.products_galery.Where(p => p.products_id == item.products_id).Count() > 0)
                {
                    thumb = db.products_galery.Where(p => p.products_id == item.products_id).FirstOrDefault().img;
                }
                else
                {
                    thumb = "404.jpg";
                }
                TrendProducts.Add(new ProductViewModel(item, thumb));
            }
            ViewBag.TrendProduct = TrendProducts;
            ViewBag.doubleFirstPost = db.news.OrderByDescending(p => p.news_id).Take(2).ToList();
            var Post = db.news.OrderByDescending(p => p.news_id).Take(10).ToList();
            if (Post.Count() > 2)
            {
                Post.RemoveRange(0, 2);
            }

            // Send data to View
            ViewBag.News = Post; 
            ViewBag.typeSearch = new List<SelectListItem>
            {
                new SelectListItem { Text = "Tìm theo tên", Value = "nameRealEstate" },
                new SelectListItem { Text = "Tìm theo địa chỉ", Value = "address" },
                new SelectListItem { Text = "Tìm trong tầm giá", Value = "price" }
            };
            ViewBag.Post = Post;
            return View();
        }

        public JsonResult GetJson()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var user = db.products.Where(p => p.isHot == true).ToList();
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string key = null, string typeSearch = null, string typeSort = null)
        {
            List<product> products = new List<product>();
            if (!string.IsNullOrEmpty(key))
            {
                
                if (typeSearch == "nameRealEstate")
                {
                    products = db.products.Where(p => p.products_name.Contains(key)).Include(p => p.collection).Include(p => p.products_galery).OrderByDescending(p => p.products_id).ToList();
                }
                else
                {
                    products = db.products.Where(p => p.address.Contains(key)).Include(p => p.collection).Include(p => p.products_galery).OrderByDescending(p => p.products_id).ToList();
                }
            }
            if (!string.IsNullOrEmpty(typeSort))
            {
                if (typeSort == "2")
                {
                    products = products.OrderByDescending(p => p.price).ToList();
                }
                else if (typeSort == "3")
                {
                    products = products.OrderBy(p => p.price).ToList();
                }
                else if (typeSort == "4")
                {
                    products = products.OrderBy(p => p.products_id).ToList();
                }
            }
            ViewBag.typeSearchList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Tìm theo tên", Value = "nameRealEstate" },
                new SelectListItem { Text = "Tìm theo địa chỉ", Value = "address" },
                new SelectListItem { Text = "Tìm trong tầm giá", Value = "price" }
            };
            ViewBag.typeSort = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bất động sản mới nhất (Mặc định)", Value = "1" },
                new SelectListItem { Text = "Giá giảm dần", Value = "2" },
                new SelectListItem { Text = "Giá tăng dần", Value = "3" },
                new SelectListItem { Text = "Bất động sản cũ nhất", Value = "4" }
            };
            ViewBag.key = key;
            ViewBag.typeSearch = typeSearch;
            return View(products);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult PolicyUse()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // POST: contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "contact_id,fullname,email,phone,conMessage,isSeen, isSaved")] contact contact)
        {
            if (ModelState.IsValid)
            {
                db.contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }
        public ActionResult Subscribed()
        {
            return View();
        }

        // POST: subscribed/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribed([Bind(Include = "subscribed_id,email")] subscribed subscribed)
        {
            if (ModelState.IsValid)
            {
                db.subscribeds.Add(subscribed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscribed);
        }
        public ActionResult usagePolicy()
        {
            return View();
        }
    }
}