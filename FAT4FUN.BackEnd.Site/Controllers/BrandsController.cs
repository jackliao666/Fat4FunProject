using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FAT4FUN.BackEnd.Site.Models;
using FAT4FUN.BackEnd.Site.Models.EFModels;

namespace FAT4FUN.BackEnd.Site.Controllers
{
    public class BrandsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Brands
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Index(string searchKeyword)
        {
            var brands = db.Brands.AsQueryable();

            // 如果提供了搜尋關鍵字，根據品牌名稱進行篩選
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                brands = brands.Where(b => b.Name.Contains(searchKeyword));
            }
                return View(brands.ToList());
        }

        // GET: Brands/Details/5
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Create([Bind(Include = "Id,Name,DisplayOrder,Status")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Edit([Bind(Include = "Id,Name,DisplayOrder,Status")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Brands/Delete/5
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Functions = "0,1,2")]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
