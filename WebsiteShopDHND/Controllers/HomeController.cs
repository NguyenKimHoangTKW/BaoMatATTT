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
            return Json(new {data = ListCategory, TotalItem = ListCategory.Count},JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddCategory(Category cat)
        {
            var status = "";
            DateTime now = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                if (db.Category.SingleOrDefault(c => c.name_cate == cat.name_cate) != null)
                {
                    status = "Tên danh mục không được bỏ trống";
                }
                else if (string.IsNullOrEmpty(cat.name_cate))
                {
                    status = "Không được bỏ trống tên danh mục";
                }
                else
                {
                    int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    cat.time_Start = unixTimestamp;
                    cat.time_Update = unixTimestamp;
                    status = "Thêm thành công";
                    db.Category.Add(cat);
                    db.SaveChanges();
                }
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(Category cat)
        {
            var status = "";
            DateTime now = DateTime.UtcNow;

            var cate = db.Category.Find(cat.id_cate);
            if (cate != null)
            {
                int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                cate.time_Update = unixTimestamp; // Update the time_Update property
                cate.id_cate = cat.id_cate;
                cate.name_cate = cat.name_cate;
                db.SaveChanges();
                status = "Cập nhật thành công";
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
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
            return Json(new { data = item ,status = "Lấy dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ClearSession()
        {
            Session.Clear();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}