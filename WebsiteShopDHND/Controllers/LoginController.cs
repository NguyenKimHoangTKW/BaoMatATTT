using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebsiteShopDHND.Models;

namespace WebsiteShopDHND.Controllers
{
    public class LoginController : Controller
    {
        ShopDHNDEntities db = new ShopDHNDEntities();
        // GET: Login
        
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("TrangChu", "Home");
        }
        [HttpPost]
        public ActionResult LoginAccount(Login login)
        {
            var status = "";

            var isRole = db.Login.SingleOrDefault(l => l.username == login.username && l.password == login.password);

            if (isRole != null && isRole.Role.name_role == "Admin")
            {
                status = "Đăng nhập thành công";
                Session["Admin"] = isRole;
            }
            else if (isRole != null && isRole.Role.name_role == "Users")
            {
                status = "Đăng nhập thành công";
                Session["User"] = isRole;
            }
            else
            {
                status = "Tên tài khoản hoặc mật khẩu không chính xác, vui lòng đăng nhập lại";
            }
            return Json(new { status = status });
        }


        [HttpPost]
        public ActionResult Register(Login login)
        {
            var password = login.password;
            var checkpassword = Request["checkpassword"];
            var status = "";
            DateTime now = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                if (db.Login.SingleOrDefault(x => x.username == login.username) != null)
                {
                    status = "Tài khoản này đã tồn tại, vui lòng nhập tài khoản khác";
                }
                else if (string.IsNullOrEmpty(login.username))
                {
                    status = "Bạn chưa nhập tên tài khoản";
                }
                else if (string.IsNullOrEmpty(login.password))
                {
                    status = "Bạn chưa nhập mật khẩu";
                }
                else if (string.IsNullOrEmpty(checkpassword))
                {
                    status = "Vui lòng nhập lại mật khẩu";
                }
                else if (password != checkpassword)
                {
                    status = "Mật khẩu nhập lại không chính xác";
                }
                else
                {
                    int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    status = "Đăng ký thành công";
                    login.id_role = 2;
                    login.creadate = unixTimestamp;
                    login.password = GetMD5(password); 
                    db.Login.Add(login);
                    db.SaveChanges();
                }
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}