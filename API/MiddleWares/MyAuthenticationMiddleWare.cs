using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.USUsers;
using Microsoft.AspNetCore.Session;

namespace API.MiddleWares
{
    public class MyAuthenticationMiddleWare
    {
        private readonly RequestDelegate _next;

        public MyAuthenticationMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Boolean flagDev = false;
            string link = context.Request.Path.Value;
            List<string> ListLink = link.Split("/").ToList();
            var OnlineUserCounter = context.Session.GetInt32("OnlineUserCounter");
            if (OnlineUserCounter == null)
            {
                //context.Session.SetString("OnlineUserCounter");
                API.Areas.Admin.Models.SiteVisit.SiteVisitService.SaveItem(DateTime.Now.ToString("yyyyMMdd"));
                context.Session.SetInt32("OnlineUserCounter", 1);
            }
            else {
                context.Session.SetInt32("OnlineUserCounter", int.Parse(OnlineUserCounter.ToString()) + 1);
            }
            
            if (flagDev)
            {
                UserToken UserToken = new UserToken()
                {
                    Id = 1,
                    IdGroup = 1,
                    IdCoQuan = 1,
                    UserName = "phucbv.dlc",
                    Email = "phucbv.dlc@vnpt.vn"
                };
                context.Request.Headers.Add("Id", UserToken.Id.ToString());
                context.Request.Headers.Add("IdGroup", UserToken.IdGroup.ToString());
                context.Request.Headers.Add("IdCoQuan", UserToken.IdCoQuan.ToString());
                context.Request.Headers.Add("UserName", UserToken.UserName);
                context.Request.Headers.Add("Email", UserToken.Email);
                await _next(context);
            }
            else {
                
               
                if (ListLink[1].ToString().ToLower() == "admin")
                {
                    
                    var data = new Models.MsgError() { Success = false, Msg = "Bạn không có quyền truy cập vào Hệ Thống." };
                    try
                    {
                        
                        if (ListLink.Count() > 3 && (ListLink[2].ToString().ToLower() == "account" && ListLink[3].ToString().ToLower() == "login"))
                        {
                            
                            await _next(context);
                        }
                        else
                        {
                            
                            var Login = context.Session.GetString("Login");
                            if (Login != null && Login != "")
                            {
                                
                                USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
                                UserToken UserToken = new UserToken()
                                {
                                    Id = MyInfo.Id,
                                    IdGroup = MyInfo.IdGroup,
                                    IdCoQuan = MyInfo.IdCoQuan,
                                    UserName = MyInfo.UserName,
                                    Email = MyInfo.Email
                                };
                                context.Session.SetString("IdCoQuan", MyInfo.IdCoQuan.ToString());
                                context.Session.SetString("TenCoQuan", MyInfo.TenCoQuan);
                                context.Session.SetString("IdUser", MyInfo.Id.ToString());
                                context.Session.SetString("IdGroup", MyInfo.IdGroup.ToString());                                
                                context.Request.Headers.Add("Id", UserToken.Id.ToString());
                                context.Request.Headers.Add("IdGroup", UserToken.IdGroup.ToString());
                                context.Request.Headers.Add("IdCoQuan", UserToken.IdCoQuan.ToString());
                                context.Request.Headers.Add("UserName", UserToken.UserName);
                                context.Request.Headers.Add("Email", UserToken.Email);
                                context.Session.SetString("LoginError", context.Session.GetString("LoginError") + JsonConvert.SerializeObject(UserToken));
                                //context.Request.Headers.Add("UserToken", JsonConvert.SerializeObject(UserToken));
                                await _next(context);
                            }
                            else
                            {
                                
                                context.Response.Redirect("/Admin/Account/Login");
                            }


                        }// if Login Action


                    }
                    catch (Exception e)
                    {
                        data.Data = e.Message;
                        await _next(context);
                        //await context.Response.WriteAsync(JsonConvert.SerializeObject(data));                        
                        //context.Response.Redirect("/Admin/Account/Login");

                    }


                }
                else
                {
                    //context.Session.SetString("LoginError", context.Session.GetString("LoginError") + "Khong hop Link.");
                    await _next(context);
                }
            }// If Dev
            

        }
       
    }

    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder UseMyAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyAuthenticationMiddleWare>();
        }
    }

}
