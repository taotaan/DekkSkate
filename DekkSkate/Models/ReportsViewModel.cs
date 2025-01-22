using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DekkSkate.Models
{
    public class ReportsViewModel
    {
        public int Report_Id { get; set; }
        public string Email { get; set; }
        public string Descript { get; set; }
        public Nullable<System.DateTime> SubmittedAt { get; set; }
        public Nullable<bool> IsRead { get; set; }
    }
}