using System;
using System.Collections.Generic;
using System.Linq;
using API.Areas.Admin.Models.DuAn;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class CSDLController : Controller
    {
        public IActionResult Index([FromQuery] SearchDuAn dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DuAnModel data = new DuAnModel() { SearchData = dto };
            data.SearchData.Status = 0;
            data.ListItems = DuAnService.GetListPaginationFront(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListItemsLoai = DuAnService.GetListItemLoai();
            data.ListItemsTrangThai = DuAnService.GetListItemTrangThai(); 
            return View(data);
        }
    }
}