using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThanhGiang_WebCuoiKi.Models;
using System.IO;
namespace ThanhGiang_WebCuoiKi.Controllers
{
    public class BaiDangController : Controller
    {
        private dbMiniShopEntities2 db = new dbMiniShopEntities2();

        // GET: BaiDang
        public async Task<ActionResult> Index(int? id)
        {
            if (id.HasValue)
            {
                tbBAIDANG bd = db.tbBAIDANGs.Find(id);
                db.tbBAIDANGs.Remove(bd);
                db.SaveChanges();
            }
            var listbaidang = db.tbBAIDANGs.Include(t => t.tbCHUYENMUC).Include(t => t.tbNHANVIEN);
            var chuyenmuc = db.tbCHUYENMUCs.ToList();
            ViewBag.chuyenmuc = chuyenmuc;
            return View(await listbaidang.ToListAsync());
        }

        // GET: BaiDang/Create
        public ActionResult Create()
        {
            ViewBag.MACHUYENMUC = new SelectList(db.tbCHUYENMUCs, "MACHUYENMUC", "TENCHUYENMUC");
            ViewBag.NGUOIDANG = new SelectList(db.tbNHANVIENs, "MANHANVIEN", "HOTEN");
            return View();
        }

        // POST: BaiDang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MABAIDANG,TIEUDE,NOIDUNG,HINHANH,NGUOIDANG,NGAYDANG,NGAYBATDAU,NGAYKETTHUC,MACHUYENMUC")] tbBAIDANG tbBAIDANG)
        {
            if (ModelState.IsValid)
            {

                db.tbBAIDANGs.Add(tbBAIDANG);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MACHUYENMUC = new SelectList(db.tbCHUYENMUCs, "MACHUYENMUC", "TENCHUYENMUC", tbBAIDANG.MACHUYENMUC);
            ViewBag.NGUOIDANG = new SelectList(db.tbNHANVIENs, "MANHANVIEN", "HOTEN", tbBAIDANG.NGUOIDANG);
            return View(tbBAIDANG);
        }

        // GET: BaiDang/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbBAIDANG tbBAIDANG = await db.tbBAIDANGs.FindAsync(id);
            if (tbBAIDANG == null)
            {
                return HttpNotFound();
            }          
            ViewBag.MACHUYENMUC = new SelectList(db.tbCHUYENMUCs, "MACHUYENMUC", "TENCHUYENMUC", tbBAIDANG.MACHUYENMUC);
            ViewBag.NGUOIDANG = new SelectList(db.tbNHANVIENs, "MANHANVIEN", "HOTEN", tbBAIDANG.NGUOIDANG);
            return View(tbBAIDANG);
        }

        // POST: BaiDang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MABAIDANG,TIEUDE,NOIDUNG,NGUOIDANG,NGAYDANG,NGAYBATDAU,NGAYKETTHUC,MACHUYENMUC")] tbBAIDANG tbBAIDANG, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {
                // Tìm bài đăng trong cơ sở dữ liệu để giữ nguyên hình ảnh cũ nếu không có tệp mới
                var existingBaiDang = await db.tbBAIDANGs.FindAsync(tbBAIDANG.MABAIDANG);

                if (existingBaiDang != null)
                {
                    if (HINHANH != null && HINHANH.ContentLength > 0)
                    {
                        // Lưu hình ảnh mới vào thư mục trên máy chủ
                        var fileName = Path.GetFileName(HINHANH.FileName);
                        var path = Path.Combine(Server.MapPath("~/img/blog"), fileName);
                        HINHANH.SaveAs(path);

                        // Xóa hình ảnh cũ nếu có (tùy chọn)
                        var oldImagePath = Server.MapPath("~/img/blog/" + existingBaiDang.HINHANH);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        // Cập nhật tên hình ảnh vào cơ sở dữ liệu
                        tbBAIDANG.HINHANH = fileName;
                    }
                    else
                    {
                        // Giữ nguyên hình ảnh cũ nếu không có tệp mới
                        tbBAIDANG.HINHANH = existingBaiDang.HINHANH;
                    }

                    db.Entry(existingBaiDang).CurrentValues.SetValues(tbBAIDANG);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.MACHUYENMUC = new SelectList(db.tbCHUYENMUCs, "MACHUYENMUC", "TENCHUYENMUC", tbBAIDANG.MACHUYENMUC);
            ViewBag.NGUOIDANG = new SelectList(db.tbNHANVIENs, "MANHANVIEN", "HOTEN", tbBAIDANG.NGUOIDANG);
            return View(tbBAIDANG);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GetBaiDang(int? chuyenmuc, string timkiem)
        {
            IQueryable<tbBAIDANG> list = db.tbBAIDANGs;
            String text = timkiem.ToLower();
            if (chuyenmuc.HasValue)
            {
                list = list.Where(p => p.MACHUYENMUC == chuyenmuc);
            }
            if (!string.IsNullOrEmpty(timkiem))
            {
                list = list.Where(p => p.TIEUDE.ToLower().Contains(text) || p.NOIDUNG.ToLower().Contains(text));
            }
            var listbd = list.ToList();
            return PartialView("_BaiDangList", listbd);
        }
    }
}
