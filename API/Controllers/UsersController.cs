using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.USUsers;
namespace API.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            USUsersModel data = new USUsersModel();
            data.ListItems = USUsersService.GetListByGroup(2);
            return View(data);
        }

        public IActionResult Detail(string alias, int Id)
        {
            USUsersModel data = new USUsersModel();
            data.Item = USUsersService.GetItemByGroup(Id,2);
            return View(data);
        }
    }
}