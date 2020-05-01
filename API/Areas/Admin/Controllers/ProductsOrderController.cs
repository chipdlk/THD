using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.ProductsOrder;
using API.Areas.Admin.Models.ProductsOrderDetail;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsOrderController : Controller
    {
        public IActionResult Index([FromQuery] SearchProductsOrder dto)
        {
            int TotalItems = 0;           
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ProductsOrderModel data = new ProductsOrderModel() { SearchData = dto };
            data.ListItems = ProductsOrderService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);

          
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            HttpContext.Session.SetString("STR_Action_Link_" + ControllerName, Request.QueryString.ToString());
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
      

            return View(data);
        }

        public IActionResult SaveItem(string Id = null, int CatId = 0, int IdCoQuan = 1)
        {
            ProductsOrderModel data = new ProductsOrderModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchProductsOrder() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};
           
            if (IdDC == 0)
            {
                data.Item = new ProductsOrder() {CreatedDate = DateTime.Now,Status=0};
            }            
            else
            {
                data.Item = ProductsOrderService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
                SearchProductsOrderDetail dts = new SearchProductsOrderDetail() { ProductOrderId = IdDC };
                data.ListDetail = ProductsOrderDetailService.GetListPagination(dts,"");
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(ProductsOrder model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            ProductsOrderModel data = new ProductsOrderModel() { Item = model };

            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    ProductsOrderService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới thành công";
                    }

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
            }
            else
            {
                //data.ListStart = ProductsOrderService.GetListStart();
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ProductsOrder model = new ProductsOrder() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    ProductsOrderService.DeleteItem(model);
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

       


    }
}