using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DekkSkate.Controllers
{
    public class FavoriteController : Controller
    {
        Entities db = new Entities();

        // GET: Favorite
        public ActionResult Index()
        {
            var currentUserEmail = User.Identity.Name;  // ใช้ Email จาก Identity
            var favorites = db.Fav.Where(f => f.Email == currentUserEmail).ToList();  // กรองตามอีเมลผู้ใช้
            return View(favorites);
        }

        [HttpPost]
        public ActionResult AddFavorites(int id)
        {
            var skateboard = db.Skateboards.Find(id);
            if (skateboard != null)
            {
                var email = User.Identity.Name;

                // ตรวจสอบว่ามีรายการนี้ใน Favorites แล้วหรือไม่
                var existingFavorite = db.Fav.FirstOrDefault(f => f.Email == email && f.SkateboardID == skateboard.SkateboardID);

                if (existingFavorite != null)
                {
                    TempData["Message"] = "ท่านมีรายการนี้แล้ว";
                    return RedirectToAction("Index");
                }

                var fav = new Fav
                {
                    Email = email,
                    SkateboardID = skateboard.SkateboardID,
                    Brand = skateboard.Brand,
                    Description = skateboard.Description,
                    Price = skateboard.Price,
                    url = skateboard.url,
                    Model = skateboard.Model
                };

                db.Fav.Add(fav);
                db.SaveChanges();
                TempData["Message"] = "เพิ่มสินค้าในรายการโปรดเรียบร้อยแล้ว";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "ไม่พบสินค้านี้";
            return RedirectToAction("Index");
        }


        // GET: Skateboards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fav fav = db.Fav.Find(id);  
            if (fav == null)
            {
                return HttpNotFound();
            }
            return View(fav);
        }

        // POST: Skateboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fav fav = db.Fav.Find(id);
            if (fav != null)
            {
                db.Fav.Remove(fav);
                db.SaveChanges();
                TempData["Message"] = "ลบสินค้าเรียบร้อยแล้ว";
            }
            else
            {
                TempData["Message"] = "ไม่พบสินค้าที่ต้องการลบ";
            }
            return RedirectToAction("Index");
        }

    }
}