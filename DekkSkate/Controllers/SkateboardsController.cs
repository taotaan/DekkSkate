using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DekkSkate;
using Microsoft.AspNet.Identity;

namespace DekkSkate.Controllers
{
    public class SkateboardsController : Controller
    {
        private Entities db = new Entities();

        // GET: Skateboards
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.BrandSortParm = String.IsNullOrEmpty(sortOrder) ? "Brand_desc" : "";

            var skateboards = from s in db.Skateboards select s;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                skateboards = skateboards.Where(s => s.Brand.ToUpper().Contains(searchString.ToUpper())
                    || s.Model.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "Brand_desc":
                    skateboards = skateboards.OrderByDescending(s => s.Brand);
                    break;
                default:
                    skateboards = skateboards.OrderBy(s => s.SkateboardID);
                    break;
            }

            return View(skateboards);
        }



        //เปรียบเทียบskate
        public JsonResult GetSkateboardDetails(int id)
        {
            var skateboard = db.Skateboards
                .Where(s => s.SkateboardID == id)
                .Select(s => new
                {
                    Name = s.Brand,
                    Description = s.Description,
                    Price = s.Price,
                    ImageURL = s.url,
                    Stock = s.stock,
                    Model = s.Model
                }).FirstOrDefault();

            if (skateboard == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(skateboard, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Favorites(int id)
        {
            var skateboard = db.Skateboards.Find(id);
            if (skateboard != null)
            {
                var userId = User.Identity.GetUserId(); // Assume a relationship between users and favorites
                var fav = new Fav
                {
                    Email = userId,
                    SkateboardID = id
                };
                db.Fav.Add(fav);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ProductList()
        {
            string Email = User.Identity.GetUserName();  // ดึง Email ของผู้ใช้ปัจจุบัน
            var skateboards = db.Skateboards.Where(s => s.Email == Email).ToList();  // แสดงเฉพาะสินค้าที่เป็นของผู้ใช้
            return View(skateboards);
        }

        // GET: Skateboards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skateboards skateboards = db.Skateboards.Find(id);
            if (skateboards == null)
            {
                return HttpNotFound();
            }
            return View(skateboards);
        }



        // GET: Skateboards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Skateboards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkateboardID,Brand,Category,Description,Price,url,CreatedBy,CreatedAt,Email,stock,Model,address,advice")] Skateboards skateboards)
        {
            skateboards.Email = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    file.SaveAs(path);
                    skateboards.url = fileName;
                }
                db.Skateboards.Add(skateboards);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skateboards);
        }

        // GET: Skateboards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skateboards skateboards = db.Skateboards.Find(id);
            if (skateboards == null)
            {
                return HttpNotFound();
            }
            return View(skateboards);
        }

        // POST: Skateboards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkateboardID,Brand,Category,Description,Price,url,CreatedBy,CreatedAt,Email,stock,Model,address,advice")] Skateboards skateboards)
        {
            var userEmail = User.Identity.GetUserName();

            skateboards.Email = userEmail;

            if (ModelState.IsValid)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    file.SaveAs(path);
                    skateboards.url = fileName;
                }
                db.Entry(skateboards).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skateboards);
        }

        // GET: Skateboards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skateboards skateboards = db.Skateboards.Find(id);
            if (skateboards == null)
            {
                return HttpNotFound();
            }
            return View(skateboards);
        }

        // POST: Skateboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Skateboards skateboards = db.Skateboards.Find(id);
            db.Skateboards.Remove(skateboards);
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