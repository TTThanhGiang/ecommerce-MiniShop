using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhGiang_WebCuoiKi.Models;

namespace ThanhGiang_WebCuoiKi.Controllers
{
    public class HomeController : Controller
    {
        dbMiniShopEntities1 db = new dbMiniShopEntities1();
        public ActionResult Index()
        {
            var sanphammoi = db.tbSANPHAMs.OrderByDescending(sp => sp.NGAYCAPNHAT).Take(8).ToList();
            ViewBag.sanphammoi = sanphammoi;

            var sanphambanchay = db.tbSANPHAMs.Where(dp => dp.SOLUONGDABAN >= 1000).Take(8).ToList();
            ViewBag.sanphambanchay = sanphambanchay;

            var sanphamdanhgiatot = db.tbSANPHAMs.Where(sp => sp.SOLUOTDANHGIA >= 100).Take(8).ToList();
            ViewBag.sanphamdanhgiatot = sanphamdanhgiatot;

            var sanphamchatluong = db.tbSANPHAMs.Where(sp => sp.CHATLUONG >= 4).Take(9).ToList();
            ViewBag.sanphamchatluong = sanphamchatluong;

            return View("Index");
        }

        //HTTP get Home/DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }

        //HTTP post Home/DangNhap
        [HttpPost]

        public ActionResult DangNhap(tbNGUOIDUNG nguoidung)
        {
            var taikhoanForm = nguoidung.TENDANGNHAP;
            var matkhauForm = nguoidung.MATKHAU;
            if (ModelState.IsValid)
            {
                var data = db.tbNGUOIDUNGs.SingleOrDefault(s => s.TENDANGNHAP.Equals(taikhoanForm) && s.MATKHAU.Equals(matkhauForm));
                if (data != null)
                {
                    //add session
                    Session["KhachHang"] = data;
                    if (data.VAITRO.Equals("customer"))
                        return RedirectToAction("Index", "Home");
                    else if(data.VAITRO.Equals("admin") || data.VAITRO.Equals("marketing") || data.VAITRO.Equals("sale"))
                        return RedirectToAction("TrangChu", "Admin");
                }
                else
                {
                    ViewBag.LoginFail = "Login failed";
                    return RedirectToAction("DangNhap", "Home");
                }
            }
            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        //HTTP post DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(tbNGUOIDUNG nguoidung)
        {
            var email = nguoidung.EMAIL;
            nguoidung.VAITRO = "customer";

            if (ModelState.IsValid)
            {
                var check = db.tbNGUOIDUNGs.SingleOrDefault(s => s.EMAIL.Equals(email));
                if (check == null)
                {
                    db.tbNGUOIDUNGs.Add(nguoidung);
                    db.SaveChanges();
                    
                    return RedirectToAction("DangNhap");
                }
                else
                {
                    ViewBag.Message = "Email already exists";
                    return View();
                }
                var makhachhang = db.tbNGUOIDUNGs.Max(ng => ng.MANGUOIDUNG) + 1;
                tbKHACHHANG kh = new tbKHACHHANG(makhachhang, nguoidung.tbKHACHHANG.HOTEN, email);
                db.tbKHACHHANGs.Add(kh);
                db.SaveChanges();
                }
                


            }

            return View();

        }
    }
}