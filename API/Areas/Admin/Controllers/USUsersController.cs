using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.USGroups;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DMChucVu;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class USUsersController : Controller
    {
        public IActionResult Index([FromQuery] SearchUSUsers dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsersModel data = new USUsersModel() { SearchData = dto };
            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1);
            data.ListItems = USUsersService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);

            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            USUsersModel data = new USUsersModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchUSUsers() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsGroups = USGroupsService.GetListSelectItems();
            data.ListItemsStatus = USUsersService.GetStatusSelectItems();
            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1);
            data.ListDMChucVu = DMChucVuService.GetListSelectItems();
            if (IdDC == 0)
            {
                data.Item = new USUsers();
            }
            else
            {
                data.Item = USUsersService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(USUsers model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            USUsersModel data = new USUsersModel() { Item = model };
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    if (model.Id == 0)
                    {
                        if (model.Password == null || model.Password == null)
                        {
                            model.Password = "Abc@123";
                        }
                        model.Password = USUsersService.GetMD5(model.Password);
                    }
                    dynamic DataSave = USUsersService.SaveItem(model);
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
            else
            {
                data.ListItemsGroups = USGroupsService.GetListSelectItems();
                data.ListItemsStatus = USUsersService.GetStatusSelectItems();
                data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 1);
                data.ListDMChucVu = DMChucVuService.GetListSelectItems();
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsers item = new USUsers() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic DataDelete = USUsersService.DeleteItem(item);
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
        public ActionResult RessetPassword(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsers item = new USUsers() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.Password = USUsersService.GetMD5("Abc@123");
                    dynamic DataDelete = USUsersService.ChangePassword(item.Id, item.Password);
                    string Msg = "Cập nhật mật khẩu thành công. Mật khẩu mặc định là <strong>" + "Abc@123" + "<strong>";
                    return Json(new MsgSuccess() { Msg = Msg });
                }
                else
                {
                    string Msg = "Cập nhật mật khẩu Không thành công. Xin vui lòng làm lại";
                    return Json(new MsgError() { Msg = Msg });
                }

            }
            catch
            {
                string Msg = "Cập nhật mật khẩu Không thành công. Xin vui lòng làm lại.";
                return Json(new MsgError() { Msg = Msg });
            }


        }
    }
}