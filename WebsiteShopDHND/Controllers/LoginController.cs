using Microsoft.Ajax.Utilities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        [ValidateInput(false)]
        public ActionResult LoginAccount(Login login)
        {
            var status = "";
            var username = EscapeInput(login.username);
            var password = EscapeInput(login.password);

            if (IsEscapedInput(username) || IsEscapedInput(password))
            {
                status = "Bạn không được nhập dữ liệu dạng Script";
                return Json(new { status = status });
            }
            else if (string.IsNullOrEmpty(username))
            {
                status = "Bạn không được bỏ trống username";
                return Json(new { status = status });
            }
            else if (string.IsNullOrEmpty(password))
            {
                status = "Bạn không được bỏ trống password";
                return Json(new { status = status });
            }
            var enpass = GetMD5(password);
            var isRole = db.Login.SingleOrDefault(l => l.username == username && l.password == enpass);

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
        [ValidateInput(false)]
        public ActionResult Register(Login login)
        {
            var username = EscapeInput(login.username);
            var password = EscapeInput(login.password);
            var checkpassword = EscapeInput(Request["checkpassword"]);
            var status = "";

            if (IsEscapedInput(username) || IsEscapedInput(password) || IsEscapedInput(checkpassword))
            {
                status = "Bạn không được nhập dữ liệu dạng Script";
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }

            DateTime now = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                if (db.Login.SingleOrDefault(x => x.username == username) != null)
                {
                    status = "Tài khoản này đã tồn tại, vui lòng nhập tài khoản khác";
                }
                else if (string.IsNullOrEmpty(username))
                {
                    status = "Bạn chưa nhập tên tài khoản";
                }
                else if (string.IsNullOrEmpty(password))
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
                    login.username = username;
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
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);
                StringBuilder byte2String = new StringBuilder();

                for (int i = 0; i < targetData.Length; i++)
                {
                    byte2String.Append(targetData[i].ToString("x2"));
                }

                return byte2String.ToString();
            }
        }

        private string EscapeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return System.Net.WebUtility.HtmlEncode(input);
        }

        private bool IsEscapedInput(string input)
        {
            if (input == null)
            {
                return false;
            }

            return input.Contains("&amp;") || input.Contains("&lt;") || input.Contains("&gt;") || input.Contains("&quot;") || input.Contains("&#x27;") || input.Contains("&#x2F;");
        }

    }
}
