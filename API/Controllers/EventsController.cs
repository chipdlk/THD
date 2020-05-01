using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Events;
using API.Areas.Admin.Models.Contacts;
using reCAPTCHA.AspNetCore;


namespace API.Controllers
{
    public class EventsController : Controller
    {
        private readonly IRecaptchaService _recaptcha;
        public EventsController(IRecaptchaService recaptcha)
        {
            _recaptcha = recaptcha;
        }
        public IActionResult Index([FromQuery] SearchEvents dto)
        {
            int TotalItems = 0;          
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.ItemsPerPage = 12;
            EventsModel data = new EventsModel() { SearchData = dto};
            data.ListItems = EventsService.GetListPaginationHome(data.SearchData, API.Models.Settings.SecretId + ControllerName);

            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);

        }

        public IActionResult Detail(string alias, int id)
        {

            EventsModel data = new EventsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchEvents() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.Item = EventsService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
               data.myContact = new ContactsModel()
               {           
                   Item = new Contacts() { CreatedDate = DateTime.Now.AddDays(1),EventId=data.Item.Id }
               };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(Contacts model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var recaptcha = await _recaptcha.Validate(Request);

            EventsModel data1 = new EventsModel();
            data1.Item = EventsService.GetItem(model.EventId, API.Models.Settings.SecretId + ControllerName);

            if (!recaptcha.success)
            {
                ModelState.AddModelError("Recaptcha", "Mã Captcha không chính xác. Vui lòng thử lại!");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.Id = 0;
                    model.CreatedBy = 0;
                    model.ModifiedBy = 0;
                    model.Type = 3;
                    try
                    {
                        ContactsService.SaveItem(model);
                        TempData["MessageSuccess"] = "Đăng ký thành công";
                    }
                    catch
                    {
                        TempData["MessageError"] = "Gửi thông tin thất bại. Xin vui lòng gửi lại";

                    }

                    data1.myContact = new ContactsModel()
                    {
                       Item = new Contacts() { CreatedDate = DateTime.Now.AddDays(1) }
                    };

                    return View(data1);

                }
            }

           
            data1.myContact = new ContactsModel()
            {
                Item = model
            };
                

            return View(data1);
        }

    }
}