using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkSkate.Models
{
    public class FavoriteViewModel
    {
        public int FavID { get; set; }
        public Nullable<int> SkateboardID { get; set; }
        public string Email { get; set; }
        public string url { get; set; }
        public string Model { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
    }
}