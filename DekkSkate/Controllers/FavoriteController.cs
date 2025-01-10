using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(db.Fav.ToList());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Favorites(int id)
        {
            var skateboard = db.Skateboards.Find(id);
            if (skateboard != null)
            {
                var Email = User.Identity.Name; // Assume a relationship between users and favorites
                var fav = new Fav
                {
                    Email = Email,
                    SkateboardID = skateboard.SkateboardID,  
                    Brand = skateboard.Brand,               
                    Description = skateboard.Description,
                    Price = skateboard.Price,
                    url = skateboard.url,               
                    Model = skateboard.Model

                };
                db.Fav.Add(fav);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}