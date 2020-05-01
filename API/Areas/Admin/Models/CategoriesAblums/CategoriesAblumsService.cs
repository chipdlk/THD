using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CategoriesAblums;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesAblums
{
    public class CategoriesAblumsService
    {
        public static List<CategoriesAblums> GetListPagination(SearchCategoriesAblums dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<CategoriesAblums>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesAblums
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
 						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
 						Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"], 						
						Ids = MyModels.Encode((int)r["Id"], SecretId)						
					}).ToList();
            }


        }

        public static List<SelectListItem> GetListItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Ablums Cha ---", Value = "0" }));
            return ListItems;

        }

        public static List<CategoriesAblums> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<CategoriesAblums>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesAblums
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
 						Status = (Boolean)r["Status"] ,
                        Featured = (Boolean)r["Featured"] ,								
					}).ToList();
            }

        }

        public static List<CategoriesAblums> GetListFeatured(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
                new string[] { "@flag" }, new object[] { "GetListFeatured"});
            if (tabl == null)
            {
                return new List<CategoriesAblums>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CategoriesAblums
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),                            
                        }).ToList();
            }

        }

        public static CategoriesAblums GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new CategoriesAblums
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
 						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
 						ParentId = (int)((r["ParentId"] == System.DBNull.Value) ? 0 : r["ParentId"]),
 						Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"], 						
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(CategoriesAblums dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
            new string[] { "@flag","@Id","@Title", "@Alias", "@Featured", "@Description","@Images","@Status","@CreatedBy","@ModifiedBy", "@ParentId" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Alias,dto.Featured, dto.Description,dto.Images,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.ParentId });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(CategoriesAblums dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }		
		public static dynamic UpdateStatus(CategoriesAblums dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(CategoriesAblums dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAblums",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

    }
}
