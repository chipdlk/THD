using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USGroups;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.USMenu;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class USGroupsController : Controller
    {        
        public IActionResult Index([FromQuery] SearchUSGroups dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USGroupsModel data = new USGroupsModel() { SearchData = dto};
            data.ListItems = USGroupsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            //data.ListItemsMenu = USGroupsService.GetListChucNang();
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            
            return View(data);
        }

        public IActionResult SaveItem(string Id=null)
        {
            USGroupsModel data = new USGroupsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());            
            data.SearchData = new SearchUSGroups() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};            
            if (IdDC == 0)
            {
                data.Item = new USGroups();
            }
            else {
                data.Item = USGroupsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            
           
            return View(data);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(USGroups model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            USGroupsModel data = new USGroupsModel() { Item = model};            
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    USGroupsService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới thành công";
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(data);
        }
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USGroups item = new USGroups() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    USGroupsService.DeleteItem(item);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else {
                    TempData["MessageError"] = "Xóa Không thành công";
                    return Json(new MsgError());
                }                
            }
            catch {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }            
        }
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USGroups item = new USGroups() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = USGroupsService.UpdateStatus(item);
                    TempData["MessageSuccess"] = "Cập nhật Trạng Thái thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Trạng Thái Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Trạng Thái không thành công";
                return Json(new MsgError());
            }
        }
        public IActionResult JsonMenus([FromBody] USGroups dto)
        {           
            return Json(USGroupsService.GetListChucNang(dto.Id));
        }
        public IActionResult JsonSaveMenus([FromBody] USGroups dto)
        {
            TempData["MessageSuccess"] = "Cập nhật thành công";
            return Json(USGroupsService.UpdateMenu(dto));
        }
    }
}
