using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.DocumentsCategories;
using API.Areas.Admin.Models.DocumentsType;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using API.Models;
using Newtonsoft.Json;


namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DocumentsController : Controller
    {
        public IActionResult Index([FromQuery] SearchDocuments dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DocumentsModel data = new DocumentsModel() { SearchData = dto };
            data.ListItems = DocumentsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            data.ListItemsStatus = DocumentsService.GetListItemsStatus();
            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            DocumentsModel data = new DocumentsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchDocuments() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            Documents Item = new Documents() {
                IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                EffectiveDateShow = DateTime.Now.ToString("dd/MM/yyyy")
            };
            if (IdDC > 0)
            {
                Item = DocumentsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
          
            data.Item = Item;

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(Documents model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            DocumentsModel data = new DocumentsModel() { Item = model };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    try
                    {
                        DocumentsService.SaveItem(model);
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
                    catch
                    {
                      
                        TempData["MessageError"] = "Lỗi khi lưu dữ liệu";
                    }

                }
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Documents model = new Documents() {  Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString())};
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DocumentsService.DeleteItem(model);
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
            Documents item = new Documents() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = DocumentsService.UpdateStatus(item);
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
