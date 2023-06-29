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
using PagedList;

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

        public ActionResult Subscribed()
        {
            var subscribed = db.subscribeds.OrderByDescending(c => c.subscribed_id).ToList();
            return View(subscribed);
        }
        public ActionResult Contact(int? page)
        {
            if (page == null) page = 1;
            var contact = db.contacts.OrderBy(c => c.isSeen).OrderByDescending(c => c.contact_id);
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(contact.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ContactDetails(int id)
        {
            var contact = db.contacts.Where(c => c.contact_id == id).FirstOrDefault();
            if (contact.isSeen == 0)
            {
                contact.isSeen++;
                db.SaveChanges();
            }
            return View(contact);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReplyContact(int Id, string To, string Subject, string Body)
        {
            if (Subject == "" || Body == "")
            {
                TempData["Error"] = "Không thành công: Vui lòng nhập đầy đủ thông tin!";
                return RedirectToAction("ContactDetails", new { Id = Id });
            }
            MessageHelpers.SendEmail(new EmailModels { To = To, Subject = Subject, Body = Body });
            TempData["Success"] = "Gửi phản hồi thành công";
            return RedirectToAction("ContactDetails", new { Id = Id });
        }
        [HttpPost, ActionName("DeleteContact")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContact(int id)
        {
            var contact = db.contacts.Find(id);
            db.contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Contact");
        }
    }
}