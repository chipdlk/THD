using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesArticles
{
    public class CategoriesArticlesService
    {
        public static List<CategoriesArticles> GetListPagination(SearchCategoriesArticles dto, string SecretId)
        {
			if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesArticles
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]), 						
 						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
 						Status = (Boolean)r["Status"],
                        NoDelete = (Boolean)r["NoDelete"],
                        Level = (int?)((r["alevel"] == System.DBNull.Value) ? null : r["alevel"]),
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),						
					}).ToList();
            }


        }

        public static List<CategoriesArticles> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesArticles
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
 						//Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        //Status = (Boolean)r["Status"],
                        //FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        /*
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
 						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
 						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),						
						TotalRows = (int)r["TotalRows"],*/
                    }).ToList();
            }

        }

        public static List<CategoriesArticles> GetListFeaturedHome()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag" }, new object[] { "GetListFeaturedHome" });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CategoriesArticles
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                            
                            ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                            Type=(int)r["Type"]
                        }).ToList();
            }

        }
        public static List<SelectListItem> GetListItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục", Value = "0" }));
            return ListItems;

        }
 


        public static CategoriesArticles GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new CategoriesArticles
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
 						Status = (Boolean)r["Status"],
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
 						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
 						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }
        public static CategoriesArticles GetItemByAlias(string Alias, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
            return (from r in tabl.AsEnumerable()
                    select new CategoriesArticles
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                        Status = (Boolean)r["Status"],
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                        Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                        Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static List<CategoriesArticles> GetListChild(int Id,int limit=100)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id","@Limit" }, new object[] { "GetListChild", Id,limit});
            return (from r in tabl.AsEnumerable()
                    select new CategoriesArticles
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                        Status = (Boolean)r["Status"],
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                        Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                        Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),                        
                    }).ToList();
        }

        public static dynamic SaveItem(CategoriesArticles dto)
        {
            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag","@Id","@Title","@Alias","@Description","@ParentId","@Status","@Deleted","@CreatedBy","@ModifiedBy","@Metadesc","@Metakey","@Metadata","@Images","@Params","@Ordering","@Hits", "@FeaturedHome" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Alias,dto.Description,dto.ParentId,dto.Status,dto.Deleted,dto.CreatedBy,dto.ModifiedBy,dto.Metadesc,dto.Metakey,dto.Metadata,dto.Images,dto.Params,dto.Ordering,dto.Hits ,dto.FeaturedHome});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(CategoriesArticles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(CategoriesArticles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateAlias(int Id, string Alias)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@Alias" },
            new object[] { "UpdateAlias", Id, Alias });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }



    }
}
