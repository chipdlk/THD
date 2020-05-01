using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesArticlesController : Controller
    {
        public IActionResult Index(string alias)
        {
            return View();
        }
    }
}