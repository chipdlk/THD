using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DMCoQuan;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    public class DMCoQuanController : Controller
    {
        public IActionResult Index(string alias)
        {
            DMCoQuan item = DMCoQuanService.GetItemByCode(alias);
            DMCoQuanModel data = new DMCoQuanModel() {
                Item = item
            };
            HttpContext.Session.SetString("IdLayoutSite", item.IdLayout.ToString());
            HttpContext.Session.SetString("IdCoQuan", item.Id.ToString());
            HttpContext.Session.SetString("Title_1", item.Title_1);
            HttpContext.Session.SetString("Title_2", item.Title_2);
            HttpContext.Session.SetString("Title_3", item.Title_3);
            return View(data);
        }
        public IActionResult Detail(string id)
        {
            return View();
        }
    }
}