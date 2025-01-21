using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkSkate.Models
{
    public class SkateboardsViewModel
    {
        public Skateboards Skateboards { get; set; }
        public List<ReviewsViewModel> Reviews { get; set; }
    }
}