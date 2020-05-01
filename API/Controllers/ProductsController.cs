using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.ProductsCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index(string alias, int id, [FromQuery] SearchProducts dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ProductsCategories categories = ProductsCategoriesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2000";
            dto.Status = 1;
            dto.ItemsPerPage = 16;
            ProductsModel data = new ProductsModel() { SearchData = dto, Categories = categories };
            data.ListItems = ProductsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);



            //data.ListItemsDanhMuc = ProductsCategoriesService.GetListItems();
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }


        public IActionResult GetListChildCat(string alias, int id, [FromQuery] SearchProducts dto)
        {
            
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ProductsCategories categories = ProductsCategoriesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            ProductsModel data = new ProductsModel() { SearchData = dto, Categories = categories };
          
            return View(data);
        }


        public IActionResult Detail(string alias,int id)
        {
            ProductsModel data = new ProductsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchProducts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsDanhMuc = ProductsCategoriesService.GetListItems();
            data.Item = ProductsService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
            ProductsCategories categories = ProductsCategoriesService.GetItem(data.Item.CatId);
            var hit = ProductsService.UpdateHit(id);

            data.Categories = categories;
            if (categories.Id != 0) {
                data.ListItems = ProductsService.GetListRelativeNews(id, categories.Id);
            }
            
            return View(data);
        }
          

    }
}