using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Events;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.Contacts;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventsController : Controller
    {        
        public IActionResult Index([FromQuery] SearchEvents dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            EventsModel data = new EventsModel() { SearchData = dto};
            data.ListItems = EventsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }


        //public IActionResult Regist(string Id )
        //{
        //    if (string.IsNullOrEmpty(Id))
        //        return RedirectToAction("Index");

        //    int TotalItems = 0;

        //    EventsModel data = new EventsModel();
        //    string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());

        //    data.Item = EventsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);

        //    data.myContact = new ContactsModel() { SearchData = new SearchContacts() { CurrentPage = 1, ItemsPerPage = 500,Type = 3 } };
        //    data.myContact.ListItems = ContactsService.GetListPagination(data.myContact.SearchData, API.Models.Settings.SecretId + ControllerName);
        //    if (data.myContact.ListItems != null && data.myContact.ListItems.Count() > 0)
        //    {
        //        TotalItems = data.myContact.ListItems[0].TotalRows;
        //    }
        //    data.myContact.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.myContact.SearchData.CurrentPage, ItemsPerPage = data.myContact.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
                       

        //    return View(data);
        //}

        public IActionResult SaveItem(string Id=null)
        {
            EventsModel data = new EventsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()); 
          
            if (IdDC == 0)
            {
                data.Item = new Events() { Status=true,DateExpiredShow=DateTime.Now.AddDays(15).ToString("dd/MM/yyyy"),DateExpired= DateTime.Now.AddDays(15) };
            }
            else {
                data.Item = EventsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
                       
            return View(data);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(Events model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());            
            EventsModel data = new EventsModel() { Item = model};
            
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    EventsService.SaveItem(model);
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
            Events model = new Events() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    EventsService.DeleteItem(model);
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
            Events item = new Events() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = EventsService.UpdateStatus(item);
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
    }
}
