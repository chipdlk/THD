using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Banners;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Banners
{
    public class BannersService
    {
        public static List<Banners> GetListPagination(SearchBanners dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@CatId", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.CatId,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Banners>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Banners
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
 						IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
 						CategoriesTitle = (string)((r["CategoriesTitle"] == System.DBNull.Value) ? null : r["CategoriesTitle"]),
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						SortOrder = (int)r["SortOrder"],
 						CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
						
					}).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsByCat(int CatId, int IdCoQuan = 1)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@CatId", "@IdCoQuan" }, new object[] { "GetListByCat", CatId, IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "-- Chọn website liên kết --", Value = "0" }));
            return ListItems;

        }

        public static List<Banners> GetListByCat(int CatId,int IdCoQuan=1)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@CatId", "@IdCoQuan" }, new object[] { "GetListByCat", CatId, IdCoQuan });
            if (tabl == null)
            {
                return new List<Banners>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Banners
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            SortOrder = (int)r["SortOrder"],
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                            Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),                            
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        }).ToList();
            }

        }

        public static List<Banners> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Banners>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Banners
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						SortOrder = (int)r["SortOrder"],
 						CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),						
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }

        }

        public static Banners GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Banners
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),                        
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						 						
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						SortOrder = (int)r["SortOrder"],
 						CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Banners dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag","@Id","@Title","@Description","@Status","@CreatedBy","@ModifiedBy","@Link","@SortOrder","@CatId","@Target","@Image" , "@IdCoQuan" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Description,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.Link,dto.SortOrder,dto.CatId,dto.Target,dto.Image ,dto.IdCoQuan});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Banners dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Banners dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
