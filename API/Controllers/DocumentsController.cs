using System;
using System.Collections.Generic;
using System.Linq;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.DocumentsCategories;
using API.Areas.Admin.Models.DocumentsType;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class DocumentsController : Controller
    {
        public IActionResult Index([FromQuery] SearchDocuments dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DocumentsModel data = new DocumentsModel() { SearchData = dto };
            data.SearchData.Status = 0;
            data.ListItems = DocumentsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            return View(data);
        }
    }
}