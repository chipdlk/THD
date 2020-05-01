using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.ImportDBOld;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.Articles;
using API.Models;
using Newtonsoft.Json;

namespace API.Areas.Admin.Controllers
{
    public class ImportDBOldController : Controller
    {
        [Area("Admin")]
        public IActionResult Index(int Category_Id = 0)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ImportDBOldModel data = new ImportDBOldModel() { };
            if (Category_Id == 1)
            {
                data.ListItems = ImportDBOldService.GetList();
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    for (int i = 0; i < data.ListItems.Count(); i++)
                    {
                        string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.ListItems[i].Title);
                        var k = API.Areas.Admin.Models.Articles.ArticlesService.UpdateAlias(data.ListItems[i].Id, Alias);
                    }
                }
            }
            if (Category_Id == 4)
            {
                data.ListItems = ImportDBOldService.GetList();
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    for (int i = 0; i < data.ListItems.Count(); i++)
                    {
                        string Introtext = API.Models.StringUtilities.Truncate(API.Models.StringUtilities.RemoveHtmlTags(data.ListItems[i].Content),250);

                        var k = API.Areas.Admin.Models.Articles.ArticlesService.UpdateIntrotext(data.ListItems[i].Id, Introtext);
                    }
                }
            }
            else if (Category_Id == 2)
            {
                List<CategoriesArticles> listCat = CategoriesArticlesService.GetList();
                if (listCat != null && listCat.Count() > 0)
                {
                    for (int i = 0; i < listCat.Count(); i++)
                    {
                        string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(listCat[i].Title);
                        var k = API.Areas.Admin.Models.CategoriesArticles.CategoriesArticlesService.UpdateAlias(listCat[i].Id, Alias);
                    }
                }
            }
            else if (Category_Id == 3)
            {
                data.ListItems = ImportDBOldService.GetList();

                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    for (int i = 0; i < data.ListItems.Count(); i++)
                    {
                        List<Articles> ListFileName = ArticlesService.GetListArticle_Files(data.ListItems[i].Id);
                        if (ListFileName != null && ListFileName.Count() > 0)
                        {
                            List<FileArticle> tmpListFile = new List<FileArticle>();
                            for (int j = 0; j < ListFileName.Count(); j++)
                            {
                                FileArticle tmp_file_article = new FileArticle() { FilePath = "/CMS/Content/TaiLieu/" + ListFileName[j].Images };
                                tmpListFile.Add(tmp_file_article);
                            }
                            if (tmpListFile != null && tmpListFile.Count() > 0)
                            {
                                ArticlesService.UpdateUpdateStr_ListFile(data.ListItems[i].Id, JsonConvert.SerializeObject(tmpListFile));
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }


            return View(data);
        }

        public IActionResult UpdateAlias()
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ImportDBOldModel data = new ImportDBOldModel() { };
            data.ListItems = ImportDBOldService.GetList();
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                for (int i = 0; i < data.ListItems.Count(); i++)
                {
                    string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.ListItems[i].Title);
                    var k = API.Areas.Admin.Models.Articles.ArticlesService.UpdateAlias(data.ListItems[i].Id, Alias);
                }
            }
            return View(data);
        }
    }
}