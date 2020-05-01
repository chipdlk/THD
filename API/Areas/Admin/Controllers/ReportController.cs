using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;


        public ReportController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [Area("Admin")]
        public IActionResult Index([FromQuery] SearchArticles dto, string btn = "search")
        {
            int TotalItems = 0;

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.SearchData.ItemsPerPage = 50;
            data.ListItems = ArticlesService.GetListReport(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            data.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4);
            data.ListItemsCreatedBy = API.Areas.Admin.Models.USUsers.USUsersService.GetListAll();
            data.ListItemType = ArticlesService.GetListItemsType(true);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            HttpContext.Session.SetString("STR_Action_Link_" + ControllerName, Request.QueryString.ToString());
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            //--Xuất excel
            if (btn == "excel")
            {
                try
                {
                    //    var l = ThongKeRepository.GetList(page, 50000, ArticleCatId, login.Role, ListCheck, tungay, denngay);
                    var workbook = new XLWorkbook();
                    var ws = workbook.Worksheets.Add("DanhSach");
                    int i = 1;
                    ws.Cell("A1").Value = "STT";
                    ws.Cell("B1").Value = "Tên bài viết";
                    ws.Cell("C1").Value = "Ngày đăng";
                    ws.Cell("D1").Value = "Người đăng";
                    ws.Cell("E1").Value = "Loại tin bài";
                    ws.Cell("F1").Value = "Danh mục";
                    ws.Cell("G1").Value = "Nguồn";
                    var rngTable = ws.Range("A1:G1");
                    rngTable.Style.Font.Bold = true;
                    var search2 = data.SearchData;
                    search2.ItemsPerPage = 100000;
                    var l = ArticlesService.GetListReport(search2, API.Models.Settings.SecretId + ControllerName);

                    foreach (var item in l)
                    {
                        i++;

                        ws.Cell("A" + i).Value = i - 1; ;
                        ws.Cell("B" + i).Value = item.Title;
                        ws.Cell("C" + i).Value = "Ngày: " +DateTime.Parse(item.CreatedDate.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        ws.Cell("D" + i).Value = item.CreatedByName;
                        ws.Cell("E" + i).Value =   (item.TypeId == 1 ? "Tin tự biên tập" : "Tin đăng lại");
                        ws.Cell("F" + i).Value = item.Category;
                        ws.Cell("G" + i).Value = item.SourceLink;
                    }
                    

                    ws.Columns().AdjustToContents();
                    
                    var path = Path.Combine(_hostingEnvironment.WebRootPath, "CMS");
                    var filename = "ThongKe_" + DateTime.Now.Ticks + ".xlsx";
                    workbook.SaveAs(System.IO.Path.Combine(path, filename));
                    string filepath = Path.Combine(path, filename);
                    FileInfo file = new FileInfo(filepath);
                    if (file.Exists)
                    {
                        
                            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                            return File(fileBytes, "application/x-msdownload", filename);
                            //System.IO.File.Delete(filepath);
                    }
                }
                catch (Exception e)
                {
                    return View(data);
                }
            }
                

                return View(data);
        }
    }
}