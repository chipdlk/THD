using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ArticlesController : Controller
    {
        public IActionResult Index(string alias)
        {
            return View();
        }

        public IActionResult KHCNDS()
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(296, API.Models.Settings.SecretId + ControllerName);
            ArticlesModel data = new ArticlesModel() { Categories = categories };  
            return View(data);
        }



        public IActionResult GetByCat(string alias,int id, [FromQuery] SearchArticles dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "") {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
            
            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2000";
            dto.Status = 1;
            dto.ItemsPerPage = 15;
            ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories  };
            data.ListItems = ArticlesService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);



            //data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult GetListChildCat(string alias, int id, [FromQuery] SearchArticles dto)
        {
            
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories };
          
            return View(data);
        }


        public IActionResult Detail(string alias,int id)
        {
            ArticlesModel data = new ArticlesModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchArticles() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            data.Item = ArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
            CategoriesArticles categories = CategoriesArticlesService.GetItem(data.Item.CatId);
            var hit = ArticlesService.UpdateHit(id);

            data.Categories = categories;
            if (categories.Id != 0) {
                data.ListItems = ArticlesService.GetListRelativeNews(alias, categories.Id);
            }
            
            return View(data);
        }


        public IActionResult GetDetailJS(int id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var Item = ArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName); 
            return Json(Item);
        }


        public IActionResult GetListByCatSimple([FromQuery]int CatId)
        {
            var list = ArticlesService.GetListByCatSimple(CatId, 10);
            return Json(list);
           
        }
    }
}