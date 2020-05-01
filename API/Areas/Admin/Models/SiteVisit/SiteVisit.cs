using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.SiteVisit
{
    public class SiteVisit
    {
        public int SiteVisitedCounter { get; set; }
        public int Id { get; set; }
    }
    public class SiteVisitDetail
    {
        public DateTime DateCreated { get; set; }
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ShowDateCreated { get; set; }
    }
    public class SiteVisitResult
    {
        public int Total { get; set; }
        public int DateNow { get; set; }
        public int DateOfWeek { get; set; }
        public int Yesterday { get; set; }
        public int OnlineUserCounter { get; set; }
    }
}
