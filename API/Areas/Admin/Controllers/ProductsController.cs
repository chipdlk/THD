using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Products;
using API.Models;
using API.Areas.Admin.Models.ProductsCategories;
using API.Models.Utilities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {


        public IActionResult Index([FromQuery] SearchProducts dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            ProductsModel data = new ProductsModel() { SearchData = dto };
            data.ListItems = ProductsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            data.ListItemsDanhMuc = ProductsCategoriesService.GetListItems();
          
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            HttpContext.Session.SetString("STR_Action_Link_" + ControllerName, Request.QueryString.ToString());
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

       

        public IActionResult GetItem(string Id = null)
        {
            Products Item = new Products();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            if (IdDC > 0)
            {
                Item = ProductsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            return Json(Item);
        }

        public IActionResult SaveItem(string Id = null)
        {
            ProductsModel data = new ProductsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchProducts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsDanhMuc = ProductsCategoriesService.GetListItems();
            Products Item = new Products() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") ,Status=true};
            if (IdDC > 0)
            {
                Item = ProductsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }

            data.Item = Item;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveItem(Products data)
        {
            ProductsModel model = new ProductsModel() { Item = data };
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, API.Models.Settings.SecretId + ControllerName).ToString());

            if (ModelState.IsValid)
            {
                if (data.Alias == null || data.Alias == "")
                {
                    model.Item.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.Title);
                }
                if (model.Item.Icon != null)
                {
                    var Image = await FileHelpers.ProcessFormFile(model.Item.Icon, ModelState);
                    if (Image.Length > 0)
                    { model.Item.Images = Image; }
                }
                if (model.Item.Id == IdDC)
                {
                    model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
                    try
                    {
                        ProductsService.SaveItem(model.Item);
                        TempData["MessageSuccess"] = "Cập nhật thành công";

                        string Str_Url = HttpContext.Session.GetString("STR_Action_Link_" + ControllerName);
                        if (Str_Url != null && Str_Url != "")
                        {
                            return Redirect("/Admin/" + this.ControllerContext.RouteData.Values["controller"].ToString() + "/Index" + Str_Url);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    catch
                    {

                    }

                }
            }
            model.SearchData = new SearchProducts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            model.ListItemsDanhMuc = ProductsCategoriesService.GetListItems();
            model.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            Products model = new Products() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    ProductsService.DeleteItem(model);
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
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            Products item = new Products() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = ProductsService.UpdateStatus(item);
                    if (item.Status == true)
                    {
                        TempData["MessageSuccess"] = "Cập nhật Trạng Thái duyệt bài thành công";
                    }
                    else {
                        TempData["MessageSuccess"] = "Cập nhật Trạng Thái Hủy duyệt bài thành công";
                    }
                    
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Trạng Thái duyệt bài Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Trạng Thái không thành công";
                return Json(new MsgError());
            }
        }

       
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeatured([FromQuery] string Ids, Boolean Featured)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            Products item = new Products() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = ProductsService.UpdateFeatured(item);
                    TempData["MessageSuccess"] = "Cập nhật Featured thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Featured Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Featured không thành công";
                return Json(new MsgError());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateNotification([FromQuery] string Ids, Boolean Notification)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            Products item = new Products() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Notification = Notification };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = ProductsService.UpdateNotification(item);
                    TempData["MessageSuccess"] = "Cập nhật thông báo thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật thông báo Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật thông báo không thành công";
                return Json(new MsgError());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeaturedHome([FromQuery] string Ids, Boolean FeaturedHome)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            Products item = new Products() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), FeaturedHome = FeaturedHome };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = ProductsService.UpdateFeaturedHome(item);
                    TempData["MessageSuccess"] = "Cập nhật Featured Home thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Featured Home Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Featured không thành công";
                return Json(new MsgError());
            }
        }
    }
}
