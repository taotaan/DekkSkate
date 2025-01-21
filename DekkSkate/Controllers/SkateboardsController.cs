using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DekkSkate;
using DekkSkate.Models;
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
            var reviewsSkate = from reviews in db.Reviews
                                     where reviews.SkateboardID == id
                                     select new ReviewsViewModel
                                     {
                                         Reviews_id = reviews.Reviews_id,
                                         Email = reviews.Email,
                                         SkateboardID = reviews.SkateboardID,
                                         comment = reviews.comment,
                                         rating = reviews.rating,
                                         Skateboards_url = skateboards.url,
                                         Skateboards_Brand = skateboards.Brand,
                                         Skateboards_Description = skateboards.Description,
                                         Skateboards_Price = skateboards.Price,
                                      
                                     };

            // สร้าง ViewModel
            var viewModel = new SkateboardsViewModel
            {
                Skateboards = skateboards,
                Reviews = reviewsSkate.ToList()
            };

            // ส่ง ViewModel ไปที่ View
            return View(viewModel);
        }

        public ActionResult ProductList()
        {
            string Email = User.Identity.GetUserName();  // ดึง Email ของผู้ใช้ปัจจุบัน
            var skateboards = db.Skateboards.Where(s => s.Email == Email).ToList();  // แสดงเฉพาะสินค้าที่เป็นของผู้ใช้
            return View(skateboards);
        }

        // GET: Skateboards/Create
        public ActionResult Create()
        {
            ViewBag.Category = new List<SelectListItem>
{
                new SelectListItem { Text = "Skateboard", Value = "Skateboard" },
                new SelectListItem { Text = "Wheel", Value = "Wheel" },
                new SelectListItem { Text = "Skateboard Sheet", Value = "Skateboard Sheet" },
                new SelectListItem { Text = "Wheel Axle", Value = "Wheel Axle" },
                new SelectListItem { Text = "Knee/Elbow Pad", Value = "Knee/Elbow Pad" },
                new SelectListItem { Text = "Helmet", Value = "Helmet" },
                new SelectListItem { Text = "Balance Board", Value = "Balance Board" }
};

            return View();
        }

        // POST: Skateboards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkateboardID,Brand,Category,Description,Price,url,CreatedBy,CreatedAt,Email,stock,Model,address,suggest")] Skateboards skateboards)
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
                try {
                    db.Skateboards.Add(skateboards);
                    db.SaveChanges();
                }

                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine($"Entity: {eve.Entry.Entity.GetType().Name}, State: {eve.Entry.State}");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                        }
                    }
                    throw;
                }


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
        public ActionResult Edit([Bind(Include = "SkateboardID,Brand,Category,Description,Price,url,CreatedBy,CreatedAt,Email,stock,Model,address,suggest")] Skateboards skateboards)
        {
            var userEmail = User.Identity.GetUserName();

            skateboards.Email = userEmail;

            if (ModelState.IsValid)
            {
                var file = Request.Files.Count > 0 ? Request.Files[0] : null;

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

        public ActionResult Compare(int? firstId, int? secondId)
        {
            // ดึงรายการสเก็ตบอร์ดทั้งหมด
            var skateboards = db.Skateboards.ToList();

            // ตรวจสอบว่ามีข้อมูลหรือไม่
            if (!skateboards.Any())
            {
                ViewBag.Error = "ไม่พบข้อมูลในฐานข้อมูล";
                return View(new List<Skateboards>());
            }

            var selectedSkateboards = new List<Skateboards>();

            if (firstId.HasValue)
            {
                var firstSkateboard = skateboards.FirstOrDefault(s => s.SkateboardID == firstId.Value);
                if (firstSkateboard != null)
                    selectedSkateboards.Add(firstSkateboard);
            }

            if (secondId.HasValue)
            {
                var secondSkateboard = skateboards.FirstOrDefault(s => s.SkateboardID == secondId.Value);
                if (secondSkateboard != null)
                    selectedSkateboards.Add(secondSkateboard);
            }

            ViewBag.Skateboards = skateboards;

            return View(selectedSkateboards);

        }

        public ActionResult Wheel()
        {
            var wheel = db.Skateboards.Where(s => s.Category == "Wheel").ToList();

            if (!wheel.Any()) // ตรวจสอบว่ามีสินค้าในรายการหรือไม่
            {
                ViewBag.Message = "ไม่มีสินค้า";
            }

            return View(wheel);
        }

        public ActionResult Sheet()
        {

            var sheet = db.Skateboards.Where(s => s.Category == "Skateboard Sheet").ToList();
            return View(sheet);
        }

        public ActionResult WheelAxle()
        {

            var wheelAxle = db.Skateboards.Where(s => s.Category == "Wheel Axle").ToList();
            return View(wheelAxle);
        }

        public ActionResult KneePad()
        {

            var kneePad = db.Skateboards.Where(s => s.Category == "Knee/Elbow Pad").ToList();
            return View(kneePad);
        }

        public ActionResult Helmet()
        {

            var helmet = db.Skateboards.Where(s => s.Category == "Helmet").ToList();
            return View(helmet);
        }

        public ActionResult BalanceBoard()
        {

            var balanceBoard = db.Skateboards.Where(s => s.Category == "Balance Board").ToList();
            return View(balanceBoard);
        }
    }
}
