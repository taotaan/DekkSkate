using DekkSkate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DekkSkate.Controllers
{
    public class DashboardsController : Controller
    {
        // GET: Dashboards
        private Entities db = new Entities();

        public ActionResult Index()
        {
            var userRolesCount = db.AspNetUsers
      .GroupBy(u => u.AspNetRoles.FirstOrDefault().Name) // Group by Role Name
      .Select(g => new
      {
          Role = g.Key,
          Count = g.Count()
      })
      .ToDictionary(g => g.Role, g => g.Count);


            var viewModel = new DashboardsViewModel
            {
                TotalUsers = db.AspNetUsers.Count(),
                TotalSkateboards = db.Skateboards.Count(),
                TotalReviews = db.Reviews.Count(),
                Skateboards = db.Skateboards.GroupBy(s => s.Brand)
                                             .ToDictionary(g => g.Key, g => g.Count()),
                ReviewsViewModel = db.Reviews.GroupBy(r => r.comment)
                          .ToDictionary(g => g.Key, g => g.Average(r => r.rating).GetValueOrDefault()),
                ReportsViewModel = db.Reports.Select(r => r.Descript).ToList(),
                FavoriteViewModel = db.Fav.Select(f => f.Model).ToList(),
                UserRolesCount = userRolesCount
            };
            return View(viewModel);
        }
    }
}