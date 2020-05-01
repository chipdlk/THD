using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.ProductsOrder;
using API.Areas.Admin.Models.ProductsOrderDetail;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using reCAPTCHA.AspNetCore;

namespace API.Controllers
{
    public class ProductsOrderController : Controller
    {
        private object context;
        private readonly IRecaptchaService _recaptcha;
        public ProductsOrderController(IRecaptchaService recaptcha)
        {
            _recaptcha = recaptcha;
        }

        public IActionResult Index()
        {
            ProductsOrderDetailModel item = new ProductsOrderDetailModel();
            item.ListItems = new List<ProductsOrderDetail>();
            //Kiem tra session
            TempData["CART"] = 0;
            var str = HttpContext.Session.GetString("ProductOrderList");
            if (!string.IsNullOrEmpty(str))
            {
                item.ListItems = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
                TempData["CART"] = 1;
            }
            return View(item);
        }



        [HttpPost]
        public IActionResult Index(List<ProductsOrderUpdate> listUpdate)
        {

            List<ProductsOrderDetail> list = new List<ProductsOrderDetail>();
            //Kiem tra session
            var str = HttpContext.Session.GetString("ProductOrderList");
            if (!string.IsNullOrEmpty(str))
            {
                list = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
            }


            if (list != null && list.Count > 0)
            {
                foreach (var item in listUpdate)
                 {
              
                    //Kiem tra da ton tai chua
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ProductId == item.ProductId)
                            list[i].Quantity = item.Quantity;
                    }

                }
            }                      

            // SET SESSION
            HttpContext.Session.SetString("ProductOrderList", JsonConvert.SerializeObject(list));
            TempData["MessageSuccess"] = "Cập nhật thành công";

            return RedirectToAction("Index");
        }



        public IActionResult CheckOut()
        {
            ProductsOrderModel item = new ProductsOrderModel();
            item.ListDetail = new List<ProductsOrderDetail>();
            //Kiem tra session
            var str = HttpContext.Session.GetString("ProductOrderList");
            TempData["CART"] = 0;
            if (!string.IsNullOrEmpty(str))
            {
                item.ListDetail = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
                TempData["CART"] = 1;
            }
            
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(ProductsOrder model)
        {
            var recaptcha = await _recaptcha.Validate(Request);

            if (!recaptcha.success)
            {
                ModelState.AddModelError("Recaptcha", "Mã Captcha không chính xác. Vui lòng thử lại!");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.Id = 0;
                    model.Status = 0;
                    try
                    {
                        int PId = (int)(ProductsOrderService.SaveItem(model).N);

                        //Thêm detail
                        List<ProductsOrderDetail> listDT = new List<ProductsOrderDetail>();
                        //Kiem tra session
                        var str1 = HttpContext.Session.GetString("ProductOrderList");
                        if (!string.IsNullOrEmpty(str1))
                        {
                            listDT = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str1);
                        }

                        foreach (var itemDT in listDT)
                        {
                            itemDT.ProductOrderId = PId;
                            itemDT.Id = 0;
                            ProductsOrderDetailService.SaveItem(itemDT);
                        }

                        HttpContext.Session.SetString("ProductOrderList","");
                        TempData["MessageSuccess"] = "Đặt hàng thành công. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.";
                    }
                    catch
                    {
                        TempData["MessageError"] = "Đặt hàng thất bại. Xin vui lòng thử lại";

                    }
                    return RedirectToAction("CheckOut");

                }
            }

            ProductsOrderModel item = new ProductsOrderModel();
            item.ListDetail = new List<ProductsOrderDetail>();
            //Kiem tra session
            var str = HttpContext.Session.GetString("ProductOrderList");
            if (!string.IsNullOrEmpty(str))
            {
                item.ListDetail = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
            }
            item.Item = model;
            return View(item);

        }



        [HttpPost]
        public ActionResult AddCart([FromQuery] int ProductId, int Quantity)
        {
            try
            {
                Products item = ProductsService.GetItem(ProductId);
                if (item != null)
                {
                    List<ProductsOrderDetail> list = new List<ProductsOrderDetail>();

                    //Kiem tra session
                    var str = HttpContext.Session.GetString("ProductOrderList");
                    if (!string.IsNullOrEmpty(str))
                    {
                        list = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
                    }

                    if (list != null && list.Count > 0)
                    {
                        var check = -1;
                        //Kiem tra da ton tai chua
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].ProductId == ProductId)                           
                                check = i;                               
                            
                        }
                        if(check==-1)
                        {
                            //Them dong moi
                            ProductsOrderDetail dt = new ProductsOrderDetail();
                            dt.ProductId = item.Id;
                            dt.Name = item.Title;
                            dt.Price = (item.Price == null ? 0 : item.Price);
                            dt.Quantity = Quantity;
                            dt.Unit = "";
                            list.Add(dt);
                        }
                        else
                        {
                            list[check].Quantity = list[check].Quantity + Quantity;
                        }

                    }
                    else
                    {
                        //Them dong moi
                        ProductsOrderDetail dt = new ProductsOrderDetail();
                        dt.ProductId = item.Id;
                        dt.Name = item.Title;
                        dt.Price = (item.Price == null ? 0 : item.Price);
                        dt.Quantity = Quantity;
                        dt.Unit = "";
                        list.Add(dt);
                    }

                    // SET SESSION
                    HttpContext.Session.SetString("ProductOrderList", JsonConvert.SerializeObject(list));
                    return Json(new MsgSuccess());

                }
                else
                {
                    return Json(new MsgError());
                }

            }
            catch
            {
                return Json(new MsgError());
            }
        }


        [HttpPost]
        public IActionResult RemoveItemCart([FromQuery] int ProductId)
        {
            try
            {
                List<ProductsOrderDetail> list = new List<ProductsOrderDetail>();
                //Kiem tra session
                var str = HttpContext.Session.GetString("ProductOrderList");
                if (!string.IsNullOrEmpty(str))
                {
                    list = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
                }

                //remove
                if (list != null && list.Count > 0)
                {
                    //Kiem tra da ton tai chua
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ProductId == ProductId)
                        {
                            list.Remove(list[i]);
                        }

                    }
                }
                // SET SESSION
                HttpContext.Session.SetString("ProductOrderList", JsonConvert.SerializeObject(list));
                TempData["MessageSuccess"] = "Xóa thành công";
                return Json(new MsgSuccess());
            }
            catch
            {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }

        }



        [HttpPost]
        public ActionResult UpdateCart([FromQuery] int ProductId, int Quantity)
        {
            try
            {
                List<ProductsOrderDetail> list = new List<ProductsOrderDetail>();
                    //Kiem tra session
                    var str = HttpContext.Session.GetString("ProductOrderList");
                    if (!string.IsNullOrEmpty(str))
                    {
                        list = JsonConvert.DeserializeObject<List<ProductsOrderDetail>>(str);
                    }

                    if (list != null && list.Count > 0)
                    {
                     
                        //Kiem tra da ton tai chua
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].ProductId == ProductId)
                            list[i].Quantity = Quantity;
                         }
                                            
                    }                    

                    // SET SESSION
                    HttpContext.Session.SetString("ProductOrderList", JsonConvert.SerializeObject(list));
                    TempData["MessageSuccess"] = "Cập nhật thành công";
                    return Json(new MsgSuccess());
            }
            catch
            {
                return Json(new MsgError());
            }
        }

    }
}