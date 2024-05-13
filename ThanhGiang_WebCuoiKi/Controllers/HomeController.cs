using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThanhGiang_WebCuoiKi.Models;
using ThanhGiang_WebCuoiKi.App_Start;

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

        public ActionResult ChiTietSanPham(decimal masanpham)
        {
            if(masanpham == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            tbSANPHAM tbsanpham = db.tbSANPHAMs.Find(masanpham);

           
            return View(tbsanpham);
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
            return View();
        }
        
        public ActionResult AddToCart(int masanpham, int? soluong)
        {
            var sanpham = db.tbSANPHAMs.FirstOrDefault(sp => sp.MASANPHAM == masanpham);
            if(sanpham != null)
            {
                List<ModelSanpham> cart;
                if(HttpContext.Session["Cart"] == null)
                {
                    cart = new List<ModelSanpham>();
                }
                else
                {
                    cart = (List<ModelSanpham>)HttpContext.Session["Cart"];
                }
                var spdatontai = cart.Where(sp => sp.MaSanPham == masanpham).FirstOrDefault();
                if(spdatontai != null)
                {
                    if(soluong.HasValue && soluong == 0)
                    {
                        cart.Remove(spdatontai);
                    }
                    else
                    {
                        spdatontai.SoLuong = Convert.ToInt32(!soluong.HasValue ? 1 : soluong);
                    }
                }
                else
                {
                    cart.Add(new ModelSanpham
                    {
                        MaSanPham = sanpham.MASANPHAM,
                        TenSanPham = sanpham.TENSANPHAM,
                        Gia = Convert.ToDecimal(sanpham.DONGIA),
                        HinhAnh = sanpham.HINHANH,
                        SoLuong = Convert.ToInt32(!soluong.HasValue ? 1 : soluong),
                        MoTa = sanpham.MOTA
                    });
                }
                HttpContext.Session["Cart"] = cart;
            }

            return RedirectToAction("Cart");
        }
        public ActionResult Cart()
        {

            List<ModelSanpham> cart = HttpContext.Session["Cart"] != null ? (List<ModelSanpham>)HttpContext.Session["Cart"] : new List<ModelSanpham>();
            decimal TOTAL_PRICE = 0;
            foreach (ModelSanpham p in cart)
            {
                TOTAL_PRICE += p.Gia * p.SoLuong;
            }
            ViewBag.TOTAL_PRICE = TOTAL_PRICE;
            return View(cart);
        }
        public ActionResult DeleteCartItem(int masanpham)
        {
            List<ModelSanpham> cart;
            cart = (List<ModelSanpham>)HttpContext.Session["Cart"];
            if (cart != null)
            {
                var productToRemove = cart.FirstOrDefault(p => p.MaSanPham == masanpham);
                if (productToRemove != null)
                {
                    cart.Remove(productToRemove);
                    // cập nhật lại
                    Session["Cart"] = cart;
                }
            }
            return RedirectToAction("Cart");
        }

        public ActionResult Blog(int? machuyenmuc)
        {
            List<tbBAIDANG> listbaidang;
            if (TempData["DanhSachTimKiem"] != null)
            {
                listbaidang = TempData["DanhSachTimKiem"] as List<tbBAIDANG>;
                TempData.Remove("DanhSachTimKiem"); // Xóa TempData sau khi sử dụng để tránh lặp lại dữ liệu
            }
            else
            {
                if (machuyenmuc != null)
                {
                    listbaidang = db.tbBAIDANGs.Where(bd => bd.MACHUYENMUC == machuyenmuc).ToList();
                }
                else
                {
                    listbaidang = db.tbBAIDANGs.ToList();
                }
            }

            var danhMucBaiViet = db.Database.SqlQuery<DanhMucBaiViet>(
                "SELECT d.MACHUYENMUC,d.TENCHUYENMUC, COUNT(s.MABAIDANG) AS SoLuongBaiViet " +
                "FROM tbCHUYENMUC d " +
                "LEFT JOIN tbBAIDANG s ON d.MACHUYENMUC = s.MACHUYENMUC " +
                "GROUP BY d.MACHUYENMUC,d.TENCHUYENMUC"
                )
            .ToList();

            
            ViewBag.DanhMucBaiViet= danhMucBaiViet;
            return View(listbaidang);
        }
        [HttpPost]
        public ActionResult TimKiemBlog(string text)
        {
            string input = text.ToLower();
            var ds = db.tbBAIDANGs.Where(bd => bd.TIEUDE.ToLower().Contains(input) || bd.NOIDUNG.ToLower().Contains(input) || bd.tbCHUYENMUC.TENCHUYENMUC.Contains(input)).ToList();
            TempData["DanhSachTimKiem"] = ds;
            return RedirectToAction("Blog");
        }
    }
}