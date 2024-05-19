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
        public async Task<ActionResult> Create([Bind(Include = "MASANPHAM,TENSANPHAM,DONGIA,SOLUONG,HINHANH,MOTA,MADANHMUC,NGAYCAPNHAT,SOLUONGDABAN,SOLUONGTOITHIEU,SOLUOTDANHGIA,CHATLUONG")] tbSANPHAM tbSANPHAM)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<ActionResult> Edit([Bind(Include = "MASANPHAM,TENSANPHAM,DONGIA,SOLUONG,HINHANH,MOTA,MADANHMUC,NGAYCAPNHAT,SOLUONGDABAN,SOLUONGTOITHIEU,SOLUOTDANHGIA,CHATLUONG")] tbSANPHAM tbSANPHAM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbSANPHAM).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
