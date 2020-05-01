using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.USGroups;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using API.Areas.Admin.Models.DMChucVu;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Info");
            
        }
        public IActionResult ChangePassword()
        {
            ChangePassword Model = new ChangePassword() {
                Id = int.Parse(HttpContext.Request.Headers["Id"])
            };
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePassword Model)
        {
            int IDUSER = int.Parse(HttpContext.Request.Headers["Id"]);
            try
            {

                string pass = USUsersService.GetMD5(Model.OldPassword);
                Model.NewPassword = Model.NewPassword.Trim();
                Model.RePassword = Model.RePassword.Trim();
                Boolean StrongPass = USUsersService.ValidateStrongPassword(Model.NewPassword);
                if (Model.Id == IDUSER)
                {
                    if (StrongPass)
                    {
                        if (Model.NewPassword == Model.RePassword)
                        {
                            USUsers item = USUsersService.CheckPassword(IDUSER, pass);
                            if (item != null && item.Password == pass)
                            {
                                string new_pass = USUsersService.GetMD5(Model.NewPassword);
                                var result = USUsersService.ChangePassword(IDUSER, new_pass);
                                TempData["MessageSuccess"] = "Thay đổi Mật khẩu thành công";                                
                            }
                            else
                            {
                                TempData["MessageError"] = "Mật khẩu cũ không chính xác";                                
                            }
                        }
                        else
                        {
                            TempData["MessageError"] = "Mật khẩu mới và mật khẩu Nhập lại không giống nhau";                            
                        }
                    }
                    else
                    {
                        TempData["MessageError"] = "Mật khẩu quá đơn giản. Độ dài Mật khẩu phải lới hơn 6, có các ký tự đặc biệt";                        
                    }
                }
                else
                {
                    TempData["MessageError"] = "Thay đổi Mật khẩu Không thành công";
                }
                             
            }
            catch (Exception e)
            {
                TempData["MessageError"] = e.Message;                
            }
            return View(new ChangePassword());
        }

        public IActionResult Info()
        {

            USUsersModel data = new USUsersModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = int.Parse(HttpContext.Request.Headers["Id"]);
            data.SearchData = new SearchUSUsers() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsGroups = USGroupsService.GetListSelectItems();
            data.ListItemsStatus = USUsersService.GetStatusSelectItems();
            data.ListDMChucVu = DMChucVuService.GetListSelectItems();
            if (IdDC == 0)
            {
                data.Item = new USUsers();
            }
            else
            {
                data.Item = USUsersService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Info(USUsers model)
        {

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = int.Parse(HttpContext.Request.Headers["Id"]);
            USUsersModel data = new USUsersModel() { Item = model };
            data.ListItemsGroups = USGroupsService.GetListSelectItems();
            data.ListItemsStatus = USUsersService.GetStatusSelectItems();
            data.ListDMChucVu = DMChucVuService.GetListSelectItems();
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    if (model.Id > 0)
                    {
                        dynamic DataSave = USUsersService.SaveAccountInfo(model);                        
                        TempData["MessageSuccess"] = "Cập nhật thành công";                        
                    }
                    
                }
            }
            else
            {
                TempData["MessageError"] = "Cập nhật Không thành công";
            }
            return View(data);
        }


        public IActionResult Login()
        {
            AccountLogin model = new AccountLogin();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountLogin model)
        {
            
            if (model.UserName == null || model.Password == null || model.UserName.Trim() == "" || model.Password.Trim() == "")
            {
                
                TempData["MessageError"] = "Thông tin đăng nhập không được để trống";
            }
            else
            {
                model.UserName = model.UserName.Trim();
                HttpContext.Session.SetString("LoginError", HttpContext.Session.GetString("LoginError") + "Tim Usser");
                USUsers Item = new USUsers();
                try
                {
                    Item = USUsersService.CheckLogin(model.UserName, model.Password);
                    if (Item == null)
                    {

                        
                        TempData["MessageError"] = "Tài khoản hoặc mật khẩu không chính xác";
                    }
                    else
                    {                        
                        HttpContext.Session.SetString("Login", JsonConvert.SerializeObject(Item));                        
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception e) {
                    HttpContext.Session.SetString("LoginError", HttpContext.Session.GetString("LoginError") + e.Message);
                }
                
                
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
        public ActionResult NoAuthorize()
        {
            return View();
        }
    }
}