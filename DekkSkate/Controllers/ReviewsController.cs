using DekkSkate.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DekkSkate.Controllers
{
    public class ReviewsController : Controller
    {
        private Entities db = new Entities();
        // GET: Reviews
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Addreviews(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var skateboards = db.Skateboards.FirstOrDefault(p => p.SkateboardID == id);
            if (skateboards == null)
            {
                return HttpNotFound();
            }


            string Email = User.Identity.GetUserName();
            var user = db.AspNetUsers.FirstOrDefault(u => u.Id == Email);

            var viewModel = new ReviewsViewModel
            {
                Email = Email,
                SkateboardID = skateboards.SkateboardID,
                Skateboards_url = skateboards.url,
                Skateboards_Brand = skateboards.Brand,
                Skateboards_Price = skateboards.Price,
                rating = skateboards.rating,

            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(ReviewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var skateboards = db.Skateboards.FirstOrDefault(p => p.SkateboardID == model.SkateboardID);
                if (skateboards == null)
                {
                    ModelState.AddModelError("", "Product not found.");
                    return View(model);
                }

                var reviews = new Reviews
                {
                    Email = model.Email,
                    SkateboardID = model.SkateboardID,
                    comment = model.comment,
                   rating = model.rating

                };
                db.Reviews.Add(reviews);
                db.SaveChanges();

                // คำนวณคะแนนเฉลี่ยของรีวิวใน Product
                var averageRating = db.Reviews
                                       .Where(r => r.SkateboardID == model.SkateboardID)
                                       .Average(r => r.rating);  // ใช้ฟิลด์ rating ของ Review คำนวณ

                // ตรวจสอบว่าไม่พบรีวิวให้ตั้งค่าเป็น 0
                if (double.IsNaN((double)averageRating))
                {
                    averageRating = 0; // ถ้าไม่มีรีวิวเลย
                }

                // อัพเดต Rating ของ Product
                skateboards.rating = averageRating;

                db.Entry(skateboards).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Review success!!";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}