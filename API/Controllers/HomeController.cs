using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.Home;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("IdCoQuan","1");
            ArticlesService.Test();
            return View();
        }


        //public IActionResult Test()
        //{
        //    var listitem = ArticlesService.GetList();
        //    for (int i = 0; i < listitem.Count(); i++)
        //    {
        //        var k = API.Areas.Admin.Models.Articles.ArticlesService.UpdateAlias(listitem[i].Id,API.Models.StringUtilities.ToNameRewrite(listitem[i].Title));
        //    }
        //    return View();
        //}




        public IActionResult Search([FromQuery] string Keyword="", int CurrentPage = 1 )
        {
            int TotalItems = 0;
            SearchAll SearchAll = new SearchAll() { Keyword = Keyword ,CurrentPage = CurrentPage };
            SearchAllModel data = new SearchAllModel()
            {
                SearchData = SearchAll,
                Flag = 1,
                ListItems = SearchService.GetListPagination(SearchAll)
            };
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }

        public IActionResult SiteMap()
        {
            
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly IFileProvider _fileProvider;

        public HomeController(IHostingEnvironment env)
        {
            _fileProvider = env.WebRootFileProvider;
        }
        //[Route("/image/{width}/{height}/{*url}")]
        public IActionResult ResizeImage(string url, int width, int height)
        {

            if (width < 0 || height < 0) { return BadRequest(); }

            var imagePath = "/" + url;
            var fileInfo = _fileProvider.GetFileInfo(imagePath);
            if (!fileInfo.Exists) { return NotFound(); }

            var outputStream = new MemoryStream();
            using (var inputStream = fileInfo.CreateReadStream())
            using (Image<Rgba32> image = Image.Load(inputStream))
            {
                image.Mutate(x => x
                 .Resize(width, height));
                image.SaveAsJpeg(outputStream);
            }

            outputStream.Seek(0, SeekOrigin.Begin);
            return File(outputStream, "image/jpg");
        }
    }
}
