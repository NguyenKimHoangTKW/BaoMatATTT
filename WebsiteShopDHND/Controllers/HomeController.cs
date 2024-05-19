using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebsiteShopDHND.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.Net;
namespace WebsiteShopDHND.Controllers
{
    public class HomeController : Controller
    {
        ShopDHNDEntities db = new ShopDHNDEntities();

        public ActionResult TrangChu()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoadCategory()
        {
            var ListCategory = db.Category.Select(ct => new
            {
                MaDanhMuc = ct.id_cate,
                TenDanhMuc = ct.name_cate,
                NgayBatDau = ct.time_Start,
                CapNhatLanCuoi = ct.time_Update,
            }).ToList();
            return Json(new { data = ListCategory, TotalItem = ListCategory.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCategory(Category cat)
        {
            var status = "";
            DateTime now = DateTime.UtcNow;

            var nameCate = EscapeInput(cat.name_cate);

            
            if (ModelState.IsValid)
            {
                if (IsEscapedInput(nameCate))
                {
                    status = "Tên danh mục chứa ký tự không hợp lệ.";
                    return Json(new { status = status }, JsonRequestBehavior.AllowGet);
                }

                else if (db.Category.SingleOrDefault(c => c.name_cate == nameCate) != null)
                {
                    status = "Tên danh mục đã tồn tại.";
                }
                else if (string.IsNullOrEmpty(nameCate))
                {
                    status = "Không được bỏ trống tên danh mục";
                }
                else
                {
                    int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    cat.time_Start = unixTimestamp;
                    cat.time_Update = unixTimestamp;
                    cat.name_cate = nameCate;
                    status = "Thêm thành công";
                    db.Category.Add(cat);
                    db.SaveChanges();
                }
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Category cat)
        {
            var status = "";
            DateTime now = DateTime.UtcNow;

            var nameCate = EscapeInput(cat.name_cate);

            if (IsEscapedInput(nameCate))
            {
                status = "Tên danh mục chứa ký tự không hợp lệ.";
                return Json(new { status = status }, JsonRequestBehavior.AllowGet);
            }

            var cate = db.Category.Find(cat.id_cate);
            if (cate != null)
            {
                int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                cate.time_Update = unixTimestamp;
                cate.name_cate = nameCate;
                db.SaveChanges();
                status = "Cập nhật thành công";
            }
            else
            {
                status = "Danh mục không tồn tại.";
            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            var status = "";
            var cat = db.Category.Find(id);
            if (cat != null)
            {
                db.Category.Remove(cat);
                db.SaveChanges();
                status = "Xóa thành công";
            }
            else
            {
                status = "Danh mục không tồn tại.";
            }

            return Json(new { status = status });
        }

        [HttpGet]
        public ActionResult GetByID(int id)
        {
            var item = db.Category.Where(x => x.id_cate == id).Select(d => new
            {
                MaDanhMuc = d.id_cate,
                TenDanhMuc = d.name_cate,
                TimeStart = d.time_Start,
            }).FirstOrDefault();

            return Json(new { data = item, status = "Lấy dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClearSession()
        {
            Session.Clear();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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