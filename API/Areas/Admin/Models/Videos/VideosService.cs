using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Videos;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Videos
{
    public class VideosService
    {
        public static List<Videos> GetListPagination(SearchVideos dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Videos
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),                        
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						CatId = (int?)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
 						Status = (Boolean)r["Status"],
 						Featured = (Boolean)r["Featured"], 						
						Ids = MyModels.Encode((int)r["Id"], SecretId),
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }


        }

        public static List<SelectListItem> GetListSelectItemsType(Boolean Selected = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Đưa lên server", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Youtube", Value = "1" }));
            return ListItems;
        }

        public static List<Videos> GetListFeatured()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag"}, new object[] { "GetListFeatured" });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Videos
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                            
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            CatId = (int?)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),                            
                        }).ToList();
            }

        }

        public static List<Videos> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Videos
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						CatId = (int?)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]), 												
					}).ToList();
            }

        }

        public static Videos GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Videos
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						CatId = (int?)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
 						Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"],
 						Deleted = (Boolean)r["Deleted"],
 						CreatedBy = (int)r["CreatedBy"],
 						CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Videos dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag","@Id","@Title", "@Featured", "@Description","@Link","@CatId","@Image","@Status","@Deleted","@CreatedBy","@CreatedDate","@ModifiedBy","@ModifiedDate", "@IdType" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Featured, dto.Description,dto.Link,dto.CatId,dto.Image,dto.Status,dto.Deleted,dto.CreatedBy,dto.CreatedDate,dto.ModifiedBy,dto.ModifiedDate,dto.IdType });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Videos dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(Videos dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(Videos dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
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
