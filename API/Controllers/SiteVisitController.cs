using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.SiteVisit;
namespace API.Controllers
{
    public class SiteVisitController : Controller
    {
        public IActionResult Index()
        {
            SiteVisitDetail DateWeek = SiteVisitService.GetByDateWeek();
            SiteVisitDetail GetAll = SiteVisitService.GetAll();
            SiteVisitDetail DateNow = SiteVisitService.GetByDate(DateTime.Now);
            SiteVisitDetail LastDate = SiteVisitService.GetByDate(DateTime.Now.AddDays(-1));
            SiteVisitResult data = new SiteVisitResult()
            {
                Total  = GetAll.Amount*3
            };
            if (DateWeek != null) {
                data.DateOfWeek = DateWeek.Amount * 3;
            }
            if (DateNow != null) {
                data.DateNow = DateNow.Amount * 3;
            }
            if (LastDate != null) {
                data.Yesterday = LastDate.Amount * 3;
            }
            return Json(data);
        }
    }
}