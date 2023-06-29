using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using aspnet_mvc_real_estate.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace aspnet_mvc_real_estate.Controllers
{
    public class AdminController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();
        // GET: Admin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ViewBag.users = userManager.Users.ToList();
                ViewBag.collections = db.collections.OrderByDescending(i => i.collections_id).Take(10).ToList();
                ViewBag.product = db.products.OrderByDescending(i => i.products_id).ToList();
                ViewBag.news = db.news.OrderByDescending(i => i.news_id).ToList();
                ViewBag.contact = db.contacts.OrderByDescending(i => i.contact_id).Take(10).ToList();
                ViewBag.subscribed = db.subscribeds.OrderByDescending(i => i.subscribed_id).Take(10).Take(10).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Contact()
        {
            var contact = db.contacts.OrderByDescending(c => c.contact_id).ToList();
            return View(contact);
        }
        public ActionResult ContactDetails(int id)
        {
            var contact = db.contacts.Where(c => c.contact_id == id).FirstOrDefault();
            return View(contact);
        }

        public ActionResult Subscribed()
        {
            var subscribed = db.subscribeds.OrderByDescending(c => c.subscribed_id).ToList();
            return View(subscribed);
        }
    }
}