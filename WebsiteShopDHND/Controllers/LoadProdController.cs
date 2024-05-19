using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteShopDHND.Controllers
{
    public class LoadProdController : Controller
    {
        // GET: LoadProd
        public ActionResult LoadProd()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }
    }
}