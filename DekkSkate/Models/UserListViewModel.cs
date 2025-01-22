using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkSkate.Models
{
    public class UserListViewModel
    {
        public IEnumerable<DekkSkate.AspNetUsers> Users { get; set; }
        public IEnumerable<DekkSkate.Skateboards> Skateboards { get; set; }
    }
}