using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DMLoaiCoQuan;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DMCoQuanController : Controller
    {
        public IActionResult Index([FromQuery] SearchDMCoQuan dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMCoQuanModel data = new DMCoQuanModel();
            data.SearchData = dto;            
            data.ListItemsLoaiCoQuan = DMLoaiCoQuanService.GetListSelectItems();
            data.ListItemsLayout = DMCoQuanService.GetListLayout();
            if(dto.CategoryId == 0 && data.ListItemsLoaiCoQuan != null && data.ListItemsLoaiCoQuan.Count() > 0)
            {
                data.SearchData.CategoryId = Convert.ToInt32(data.ListItemsLoaiCoQuan[0].Value);
            }

            data.ListItems = DMCoQuanService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem(string Id = null, int CategoryId = 0,int ParentId = 0)
        {
            
            DMCoQuanModel data = new DMCoQuanModel();
                        
            data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(CategoryId);
            data.ListItemsLayout = DMCoQuanService.GetListLayout();
            data.ListItemsLoaiCoQuan = DMLoaiCoQuanService.GetListSelectItems();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + "DMCoQuan").ToString());
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDMCoQuan() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };           
            if (IdDC == 0)
            {
                data.Item = new DMCoQuan();
                data.Item.CategoryId = CategoryId;
                data.Item.ParentId = ParentId;
            }
            else
            {
                data.Item = DMCoQuanService.GetItem(IdDC, API.Models.Settings.SecretId + "DMCoQuan".ToString());
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(DMCoQuan model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            DMCoQuanModel data = new DMCoQuanModel();
            data.ListItemsLayout = DMCoQuanService.GetListLayout();
            data.ListItemsLoaiCoQuan = DMLoaiCoQuanService.GetListSelectItems();
            data.Item = model;
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Obj = DMCoQuanService.SaveItem(model);
                    if (Obj.N == -1)
                    {
                        TempData["MessageError"] = "Mã cơ quan trùng";
                        data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(model.CategoryId);
                        
                        return View(data);
                    }else if (Obj.N == -2)
                    {
                        TempData["MessageError"] = "Chọn Cơ quan cha không hợp lệ";
                        data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(model.CategoryId);
                        
                        return View(data);
                    }else
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                        return RedirectToAction("Index", new { CategoryId = model.CategoryId });
                    }                                        
                }
            }
            else
            {
                data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(model.CategoryId);
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMCoQuanModel data = new DMCoQuanModel();
            DMCoQuan item = new DMCoQuan() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (item.Id > 0)
                {                    
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DMCoQuanService.DeleteItem(item);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Xóa Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }
        }
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMCoQuan item = new DMCoQuan() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {                    
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = DMCoQuanService.UpdateStatus(item);
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