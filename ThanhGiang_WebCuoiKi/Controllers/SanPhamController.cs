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
    public class SanPhamController : Controller
    {
        private dbMiniShopEntities2 db = new dbMiniShopEntities2();

        // GET: SanPham
        public async Task<ActionResult> Index(int? id)
        {
            if (id.HasValue)
            {
                tbSANPHAM sp = db.tbSANPHAMs.Find(id);
                db.tbSANPHAMs.Remove(sp);
                db.SaveChanges();
            }
            var tbSANPHAMs = db.tbSANPHAMs.Include(t => t.tbDANHMUC);
            var listdm = db.tbDANHMUCs.ToList();
            ViewBag.listdanhmuc = listdm;
            return View(await tbSANPHAMs.ToListAsync());
        }

        // GET: SanPham/Create
        public ActionResult Create()
        {
            ViewBag.MADANHMUC = new SelectList(db.tbDANHMUCs, "MADANHMUC", "TENDANHMUC");
            return View();
        }

        // POST: SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MASANPHAM,TENSANPHAM,DONGIA,SOLUONG,HINHANH,MOTA,MADANHMUC,NGAYCAPNHAT")] tbSANPHAM tbSANPHAM, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {

                if(HINHANH != null && HINHANH.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HINHANH.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/product"), fileName);
                    HINHANH.SaveAs(path);
                    tbSANPHAM.HINHANH = fileName;
                }
                db.tbSANPHAMs.Add(tbSANPHAM);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MADANHMUC = new SelectList(db.tbDANHMUCs, "MADANHMUC", "TENDANHMUC", tbSANPHAM.MADANHMUC);
            return View(tbSANPHAM);
        }

        // GET: SanPham/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSANPHAM tbSANPHAM = await db.tbSANPHAMs.FindAsync(id);
            if (tbSANPHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.MADANHMUC = new SelectList(db.tbDANHMUCs, "MADANHMUC", "TENDANHMUC", tbSANPHAM.MADANHMUC);
            return View(tbSANPHAM);
        }

        // POST: SanPham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MASANPHAM,TENSANPHAM,DONGIA,SOLUONG,MOTA,MADANHMUC,NGAYCAPNHAT")] tbSANPHAM tbSANPHAM, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {
                var sp = await db.tbSANPHAMs.FindAsync(tbSANPHAM.MASANPHAM);

                if (sp != null)
                {
                    if (HINHANH != null && HINHANH.ContentLength > 0)
                    {
                        // Lưu hình ảnh mới vào thư mục trên máy chủ
                        var fileName = Path.GetFileName(HINHANH.FileName);
                        var path = Path.Combine(Server.MapPath("~/img/product"), fileName);
                        HINHANH.SaveAs(path);

                        // Xóa hình ảnh cũ nếu có (tùy chọn)
                        var oldImagePath = Server.MapPath("~/img/product/" + sp.HINHANH);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        // Cập nhật tên hình ảnh vào cơ sở dữ liệu
                        tbSANPHAM.HINHANH = fileName;
                    }
                    else
                    {
                        // Giữ nguyên hình ảnh cũ nếu không có tệp mới
                        tbSANPHAM.HINHANH = sp.HINHANH;
                    }

                    db.Entry(sp).CurrentValues.SetValues(tbSANPHAM);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
                ViewBag.MADANHMUC = new SelectList(db.tbDANHMUCs, "MADANHMUC", "TENDANHMUC", tbSANPHAM.MADANHMUC);
            return View(tbSANPHAM);
        }
        public ActionResult GetSanPham(int? danhmuc, string timkiem)
        {
            IQueryable<tbSANPHAM> list = db.tbSANPHAMs;
            String text = timkiem.ToLower();
            if (danhmuc.HasValue)
            {
                list = list.Where(p => p.MADANHMUC == danhmuc);
            }
            if (!string.IsNullOrEmpty(timkiem))
            {
                list = list.Where(p => p.TENSANPHAM.ToLower().Contains(text) || p.MOTA.ToLower().Contains(text));
            }
            var listsp = list.ToList();
            return PartialView("_SanPhamList", listsp);
        }
    }
}
