using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkSkate.Models
{
    public class DashboardsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalSkateboards { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<string, int> Skateboards { get; set; } // แก้ไขคำสะกด
        public Dictionary<string, double> ReviewsViewModel { get; set; }
        public List<string> ReportsViewModel { get; set; }
        public List<string> FavoriteViewModel { get; set; }
        public Dictionary<string, int> UserRolesCount { get; set; }
    }
}