using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhGiang_WebCuoiKi.Models;
namespace ThanhGiang_WebCuoiKi.Controllers
{
    public class AdminController : Controller
    {
        dbMiniShopEntities2 db = new dbMiniShopEntities2();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

    }
}