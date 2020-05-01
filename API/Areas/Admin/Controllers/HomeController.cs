using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.SYSParams;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SetSeccionMenu(int Id)
        {
            HttpContext.Session.SetInt32("IdMenu", Id);
            return Json(new MsgSuccess() { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(SYSConfig model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();            
            SYSConfigModel data = new SYSConfigModel() { Item = model };
            if (ModelState.IsValid)
            {                
                SYSParamsService.SaveConfig(model);
                TempData["MessageSuccess"] = "Cập nhật thành công";
                return RedirectToAction("SaveItem");
            }
            return View(data);
            
        }

        public IActionResult SaveItem()
        {
            SYSConfigModel Model = new SYSConfigModel() {
                Item = SYSParamsService.GetItemConfig()
            };
            return View(Model);
        }
    }
}