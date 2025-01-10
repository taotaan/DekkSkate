using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkSkate.Models
{
    public class ReviewsViewModel
    {
        public int Reviews_id { get; set; }
        public string Email { get; set; }
        public string comment { get; set; }
        public Nullable<int> SkateboardID { get; set; }
        public Nullable<double> rating { get; set; }
        public string Skateboards_url { get; set; }
        public string Skateboards_Brand { get; set; }
        public Nullable<decimal> Skateboards_Price { get; set; }
        public string Skateboards_Description { get; set; }
       
    }
}