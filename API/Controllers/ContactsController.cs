using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Contacts;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;

namespace API.Controllers
{

    public class ContactsController : Controller
    {
        private readonly IRecaptchaService _recaptcha;
        public ContactsController(IRecaptchaService recaptcha)
        {
            _recaptcha = recaptcha;
        }

        public IActionResult Booking()
        {
            ContactsModel data = new ContactsModel()
            {
                SearchData = new SearchContacts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""},
                Item = new Contacts() { CreatedDateShow = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy HH:mm") }
            };
            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(Contacts model)
        {
            //string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var recaptcha = await _recaptcha.Validate(Request);


            ContactsModel data = new ContactsModel() { Item = model };
            data.Item.Type = 2;
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
                    model.Type = 2;
                    model.CreatedDate = DateTime.ParseExact(model.CreatedDateShow, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    try
                    {
                        ContactsService.SaveItem(model);
                        TempData["MessageSuccess"] = "Đặt lịch khám thành công";
                    }
                    catch
                    {
                        TempData["MessageError"] = "Gửi thông tin thất bại. Xin vui lòng gửi lại";

                    }
                    return RedirectToAction("Booking");

                }
            }

            return View(data);
        }


        public IActionResult Index()
        {
            ContactsModel data = new ContactsModel() {
                SearchData = new SearchContacts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" },
                Item = new Contacts()
            };                        
            return View(data);            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Contacts model)
        {
            //string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var recaptcha = await _recaptcha.Validate(Request);


            ContactsModel data = new ContactsModel() { Item = model };
            data.Item.Type = 1;
            if (!recaptcha.success)
            {
                ModelState.AddModelError("Recaptcha", "Mã Captcha không chính xác. Vui lòng thử lại!");
            }
            else {
                if (ModelState.IsValid)
                {
                    model.Id = 0;
                    model.CreatedBy = 0;
                    model.ModifiedBy = 0;
                    model.Type = 1;
                    try
                    {
                        ContactsService.SaveItem(model);
                        TempData["MessageSuccess"] = "Gửi thông tin tư vấn thành công";                       
                    }
                    catch {
                        TempData["MessageError"] = "Gửi thông tin thất bại. Xin vui lòng gửi lại";
                        
                    }
                    return RedirectToAction("Index");

                }
            }
            
            return View(data);
        }

    }
}