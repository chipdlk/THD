using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Reviews;
using API.Models;

namespace API.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index([FromQuery] SearchReviews dto)
        {
            int TotalItems = 0;

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ReviewsModel data = new ReviewsModel() { SearchData = dto };
            data.ListItems = ReviewsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);


            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };


            return View(data);
        }
        public IActionResult Detail(int id,string alias="" )
        {
            ReviewsModel data = new ReviewsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();           
            data.Item = ReviewsService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

            //CategoriesArticles categories = CategoriesArticlesService.GetItem(data.Item.CatId);
            //data.Categories = categories;
            //if (categories.Id != 0)
            //{
            //    data.ListItems = ArticlesService.GetListRelativeNews(alias, categories.Id);
            //}

            return View(data);
        }
    }
}