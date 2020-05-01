using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Menus;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Menus
{
    public class MenusService
    {
        public static List<Menus> GetListPagination(SearchMenus dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword ,dto.IdCoQuan});
            if (tabl == null)
            {
                return new List<Menus>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Menus
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						CatId = (int)r["CatId"],
 						StaticId = (int)r["StaticId"],
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
 						TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? 0 : r["TenCoQuan"]),
 						ParentId = (int)r["ParentId"],
                        Type = (int)r["Type"],
                        Status = (Boolean?)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						
 						Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),						
					}).ToList();
            }


        }

        public static List<Menus> GetList(Boolean Selected = true,int IdCoQuan = 1)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetList", Convert.ToDecimal(Selected),IdCoQuan });
            if (tabl == null)
            {
                return new List<Menus>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Menus
					{
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],                        
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),                        
                        ParentId = (int)r["ParentId"],
                        Type = (int)r["Type"],
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"])                        
					}).ToList();
            }

        }
        public static List<Menus> GetListByParrent(int ParentId = 0,int IdCoQuan=1)
        {
            List<Menus> menus = new List<Menus>();

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@ParentId", "@IdCoQuan" }, new object[] { "GetListByParrent", ParentId, IdCoQuan });
            if (tabl == null)
            {
                menus = new List<Menus>();
            }
            else
            {
                menus = (from r in tabl.AsEnumerable()
                        select new Menus
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IdCoQuan = (int)r["IdCoQuan"],
                            StaticId = (int)r["StaticId"],
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            ParentId = (int)r["ParentId"],
                            Type = (int)r["Type"],
                            ArticleId = (int?)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                            ChildCount = (int)((r["ChildCount"] == System.DBNull.Value) ? null : r["ChildCount"]),
                        }).ToList();
            }
            for(int i = 0; i < menus.Count; i++)
            {
                if(menus[i].ChildCount > 0)
                {
                    menus[i].ListMenus = GetListByParrent(menus[i].Id, menus[i].IdCoQuan);
                }
            }
            return menus;
        }
        public static List<SelectListItem> GetListItems(Boolean Selected = true, int IdCoQuan=1)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Menu", Value = "0" }));
            return ListItems;

        }

        public static Menus GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Menus
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						CatId = (int)r["CatId"],
 						StaticId = (int)r["StaticId"],
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
 						ParentId = (int)r["ParentId"],
                        Type = (int)r["Type"],
                        ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						Status = (Boolean?)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
 						Deleted = (Boolean?)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
 						ArticleId = (int?)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]),
 						Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Menus dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag","@Id","@Title","@CatId","@StaticId","@Link","@ParentId","@ModifiedBy","@Status","@Deleted","@ModifiedDate","@ArticleId","@Ordering","@Icon", "@IdCoQuan","@Type" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.CatId,dto.StaticId,dto.Link,dto.ParentId,dto.ModifiedBy,dto.Status,dto.Deleted,dto.ModifiedDate,dto.ArticleId,dto.Ordering,dto.Icon,dto.IdCoQuan,dto.Type });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Menus dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(Menus dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static List<SelectListItem> GetListType()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Direct link", Value = "1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Danh mục bài viết", Value = "2" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Danh mục sản phẩm", Value = "3" }));

            return ListItems;
        }


    }
}
