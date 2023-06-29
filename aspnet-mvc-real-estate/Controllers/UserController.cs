using aspnet_mvc_real_estate.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Web.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System.Data.Entity;
using System.Net;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace aspnet_mvc_real_estate.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private mvc_real_estate_db_entity db = new mvc_real_estate_db_entity();
        // GET: User
        public ActionResult Index(string name, int? page)
        {
            if (page == null) page = 1;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.Users.OrderBy(i => i.Id);
            if (!String.IsNullOrEmpty(name))
            {
                ViewBag.Name = name;
                user = userManager.Users.Where(p => p.FullName.Contains(name)).OrderBy(i => i.Id);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(user.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UserDetails(string Id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(Id);
            ViewBag.user = user;
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        public ActionResult Edit(string Email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindByEmail(Email);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                //Lưu thông tin User vào Database
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var manager = new UserManager<ApplicationUser>(store);
                var currentUser = manager.FindByEmail(model.Email);
                currentUser.FullName = model.FullName;
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.Address = model.Address;
                currentUser.Gender = model.Gender;
                currentUser.isAdmin = model.isAdmin;
                var result = await manager.UpdateAsync(currentUser);
                var ctx = store.Context;
                ctx.SaveChanges();

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                ViewBag.Error = "Chỉnh sửa không thành công";
            }
            return View(model);
        }


        public async Task<ActionResult> Delete(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByIdAsync(id);

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["UserDeleted"] = "Xóa thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["UserDeleted"] = "Có lỗi xảy ra";
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindByEmail(email);
            ViewBag.Message = "";
            if (user != null)
            {
                Random rand = new Random();
                int randomNumber = rand.Next(0, 999999);
                string verifyCode = string.Format("{0:D6}", randomNumber);
                Session["ForgotVerifyCode"] = HashHelpers.ComputeSha512Hash(verifyCode);
                Session.Timeout = 5;
                Session["ForgotVerifyMail"] = email;
                // Send verify Email
                string Subject = "Xác minh thay đổi mật khẩu";
                string Body = "<center><img src=\"https://bizweb.dktcdn.net/100/328/362/themes/894751/assets/logo.png?1676257083798\"></center><br/>[SKYLANDS] Bạn đang yêu cầu thay đổi mật khẩu tài khoản nhân viên tại Trang Quản Trị Skylands. Vui lòng không cung cấp mã OTP cho người khác để bảo vệ tài khoản. <br /> - Mã OTP là: <b>" + verifyCode + "</b><br/>Lưu ý: Mã xác thực OTP chỉ có hiệu lực trong 5 phút.<br/>Nếu đây không phải là yêu cầu của bạn, hãy bỏ qua tin nhắn này!";
                EmailModels emailModels = new EmailModels(email, Subject, Body);
                MessageHelpers.SendEmail(emailModels);
                return RedirectToAction("VeryfiForgotPassword");
            }
            else
            {
                ViewBag.Message = "Bạn chưa nhập hoặc địa chỉ Email không tồn tại, vui lòng kiểm tra lại!";
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult VeryfiForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VeryfiForgotPassword(string VeryfiCode)
        {
            string email = Session["ForgotVerifyMail"] as string;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindByEmail(email);
            string HashCode = Session["ForgotVerifyCode"] as string;
            if (HashHelpers.VerifySha512Hash(VeryfiCode, HashCode))
            {
                Session.Remove("ForgotVerifyMail");
                Session.Remove("ForgotVerifyMail");
                string newPassword = GenerateHelpers.GenerateStrongPassword(10);
                try
                {
                    var provider = new DpapiDataProtectionProvider("ApplicationName");
                    userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("ASP.NET Identity"));
                    string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                    var result = await userManager.ResetPasswordAsync(user.Id, code, newPassword);
                    if (result.Succeeded)
                    {
                        // Send new password
                        string Subject = "Lấy lại mật khẩu thành công";
                        string Body = "<center><img src=\"https://bizweb.dktcdn.net/100/328/362/themes/894751/assets/logo.png?1676257083798\"></center><br/>[SKYLANDS] Mật khẩu mới của bạn là: " + newPassword + "<p><b>Lưu ý: hãy đổi mật khẩu ngay để đảm bảo tính bảo mật cho tài khoản của bạn!</b></p>";
                        EmailModels emailModels = new EmailModels(email, Subject, Body);
                        MessageHelpers.SendEmail(emailModels);
                        return RedirectToAction("GetPasswordSuccess");
                    }
                    else
                    {
                        ViewBag.Message = "Lấy lại mật khẩu thất bại, vui lòng thử lại!";
                    }
                }
                catch
                {
                    ViewBag.Message = "Có lỗi xảy ra! Vui lòng thử lại sau";
                }
            }
            else
            {
                ViewBag.Message = "Mã OTP không chính xác";
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetPasswordSuccess()
        {
            return View();
        }
    }
}