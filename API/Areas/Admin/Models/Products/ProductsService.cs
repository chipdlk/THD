using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Products;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace API.Areas.Admin.Models.Products
{
    public class ProductsService
    {
        public static List<Products> GetListPagination(SearchProducts dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 15;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string str_sql = "GetListPagination_Status";
            Boolean Status = true;
            if (dto.Status == -1)
            {
                str_sql = "GetListPagination";
            }
            else if (dto.Status == 0)
            {
                Status = false;
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy", "@Status" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdCoQuan, dto.AuthorId, StartDate, EndDate, dto.CreatedBy, Status });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            Category= (string)((r["Category"] == System.DBNull.Value) ? null : r["Category"]),
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            Status = (Boolean)r["Status"],
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId)

                        }).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Chưa Duyệt", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Đã Duyệt", Value = "1" }));
            return ListItems;
        }

        public static List<Products> GetListPaginationByCat(string alias, SearchProducts dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 15;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatAlias" },
                new object[] { "GetListPaginationByCat", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, alias });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            Status = (Boolean)r["Status"],
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),
                        }).ToList();
            }


        }


        public static List<Products> GetListArticle_Files(int Id)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@Id" }, new object[] { "GetListArticle_Files", Id });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            CatId = (int)r["IdArticle"],
                            Images = (string)((r["FileName"] == System.DBNull.Value) ? null : r["FileName"])
                        }).ToList();
            }

        }
      
        public static List<SelectListItem> GetListItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn sản phẩm", Value = "0" }));
            return ListItems;

        }



        public static List<Products> GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                            Status = (Boolean)r["Status"],
                            CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                            Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                            Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                            Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            Notification = (Boolean)((r["Notification"] == System.DBNull.Value) ? null : r["Notification"]),
                            Price = (int)((r["Price"] == System.DBNull.Value) ? 0 : r["Price"])
                            //TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }

        }

        public static List<Products> GetListRelativeNews(int Id, int CatId)
        {
          
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@Id", "@CatId" }, new object[] { "GetListRelativeNews", Id, CatId });
                if (tabl == null)
                {
                    return new List<Products>();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new Products
                            {
                                Id = (int)r["Id"],
                                Title = (string)r["Title"],
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                CatId = (int)r["CatId"],
                                IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                Status = (Boolean)r["Status"],        
                                PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),                            
                                Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),                              
                                Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),                               
                                Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),
                            }).ToList();
                }
            


        }
        public static List<Products> GetListHot(int IdCoQuan = 0,int Limit=5)
        {
            if (IdCoQuan == 0)
            {
                IdCoQuan = 1;
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@IdCoQuan","@Limit" }, new object[] { "GetListHot", IdCoQuan,Limit });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            Category= (string)((r["Category"] == System.DBNull.Value) ? null : r["Category"]),
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            Status = (Boolean)r["Status"],
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),
                        }).ToList();
            }

        }

        public static List<Products> GetListNotification(SearchProducts dto)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage" }, new object[] { "GetListNotification", dto.CurrentPage, dto.ItemsPerPage });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            Status = (Boolean)r["Status"],
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),
                        }).ToList();
            }

        }
                    

   

      

        public static Products GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            Products Item = (from r in tabl.AsEnumerable()
                             select new Products
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                 Notification = (Boolean)((r["Notification"] == System.DBNull.Value) ? null : r["Notification"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                                 Hit = (int)((r["Hit"] == System.DBNull.Value) ? 0 : r["Hit"]),
                                 Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),

                             }).FirstOrDefault();
            if (Item.Str_ListFile != null && Item.Str_ListFile != "")
            {
                Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
            }
            if (Item.Str_Link != null && Item.Str_Link != "")
            {
                Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
            }
            return Item;
        }


        public static Products GetItemByAlias(string Alias, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
            Products Item = (from r in tabl.AsEnumerable()
                             select new Products
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                                 Hit = (int)r["Hit"],
                                 Price = (Decimal?)((r["Price"] == System.DBNull.Value) ?null : r["Price"]),
                             }).FirstOrDefault();

            if (Item.Str_ListFile != null && Item.Str_ListFile != "")
            {
                Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
            }

            if (Item.Str_Link != null && Item.Str_Link != "")
            {
                Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
            }
            return Item;
        }

        public static dynamic SaveItem(Products dto)
        {
            string Str_ListFile = null;
            string Str_Link = null;
            List<FileArticle> ListFileArticle = new List<FileArticle>();
            List<LinkArticle> ListLinkArticle = new List<LinkArticle>();

            if (dto.ListFile != null && dto.ListFile.Count() > 0)
            {
                for (int i = 0; i < dto.ListFile.Count(); i++)
                {
                    if (dto.ListFile[i].FilePath != null && dto.ListFile[i].FilePath.Trim() != "")
                    {
                        ListFileArticle.Add(dto.ListFile[i]);
                    }
                }
                if (ListFileArticle != null && ListFileArticle.Count() > 0)
                {
                    Str_ListFile = JsonConvert.SerializeObject(ListFileArticle);
                }

            }


            if (dto.ListLinkArticle != null && dto.ListLinkArticle.Count() > 0)
            {
                for (int i = 0; i < dto.ListLinkArticle.Count(); i++)
                {
                    if (dto.ListLinkArticle[i].Title != null && dto.ListLinkArticle[i].Title.Trim() != "" && dto.ListLinkArticle[i].Status == true)
                    {
                        ListLinkArticle.Add(dto.ListLinkArticle[i]);
                    }
                }
                if (ListLinkArticle != null && ListLinkArticle.Count() > 0)
                {
                    Str_Link = JsonConvert.SerializeObject(ListLinkArticle);
                }

            }
            DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@Title", "@Alias", "@CatId", "@IntroText", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@CreatedDate", "@ModifiedDate", "@Metadesc", "@Metakey", "@Metadata", "@Language", "@Featured", "@StaticPage", "@Images", "@Params", "@Ordering", "@Deleted", "@IdCoQuan", "@FeaturedHome", "@PublishUp", "@Str_ListFile", "@Str_Link", "@AuthorId","@Notification","@Price"},
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.CatId, dto.IntroText, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.CreatedDate, dto.ModifiedDate, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Language, dto.Featured, dto.StaticPage, dto.Images, dto.Params, dto.Ordering, dto.Deleted, dto.IdCoQuan, dto.FeaturedHome, NgayDang, Str_ListFile, Str_Link, dto.AuthorId,dto.Notification,dto.Price });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Products dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Products dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic UpdateHit(int id)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id" },
            new object[] { "UpdateHit", id});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        

        public static dynamic UpdateFeatured(Products dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateNotification(Products dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@Notification", "@ModifiedBy" },
            new object[] { "UpdateNotification", dto.Id, dto.Notification, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeaturedHome(Products dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@FeaturedHome", "@ModifiedBy" },
            new object[] { "UpdateFeaturedHome", dto.Id, dto.FeaturedHome, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateAlias(int Id, string Alias)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@Alias" },
            new object[] { "UpdateAlias", Id, Alias });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateUpdateStr_ListFile(int Id, string Str_ListFile)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@Id", "@Str_ListFile" },
            new object[] { "UpdateStr_ListFile", Id, Str_ListFile });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }



    }
}
