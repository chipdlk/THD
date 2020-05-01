using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Ablums;
using API.Areas.Admin.Models.CategoriesAblums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AlbumsController : Controller
    {
        //add a comment
        public IActionResult Index([FromQuery] SearchCategoriesAblums dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesAblumsModel data = new CategoriesAblumsModel() { SearchData = dto };
            data.ListItemsAlbums = AblumsService.GetListPagination(new SearchAblums());
            List<CategoriesAblums> ListCatAblum = new List<CategoriesAblums>();
            ListCatAblum = CategoriesAblumsService.GetList();

            for (int i = 0; i < ListCatAblum.Count(); i++) {
                List<Ablums> tmp = new List<Ablums>();
                for (int j = 0; j < data.ListItemsAlbums.Count(); j++) {
                    if (ListCatAblum[i].Id == data.ListItemsAlbums[j].CatId) {
                        tmp.Add(data.ListItemsAlbums[j]);
                    }
                }
                ListCatAblum[i].ListItemsAblums = tmp;
            }

            data.ListItems = ListCatAblum;
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }
    }
}