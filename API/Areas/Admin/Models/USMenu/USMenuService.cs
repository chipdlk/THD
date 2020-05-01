using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.USMenu;
using API.Models;
namespace API.Areas.Admin.Models.USMenu
{
    public class USMenuService
    {
        public static List<USMenu> GetListPagination(SearchUSMenu dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
                new string[] { "@flag",  "@Keyword" },
                new object[] { "GetListPagination",  dto.Keyword });
            if (tabl == null)
            {
                return new List<USMenu>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new USMenu
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]), 					
 						Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
 						TreeMenu = (string)((r["TreeMenu"] == System.DBNull.Value) ? null : r["TreeMenu"]),
 						TreePath = (string)((r["TreePath"] == System.DBNull.Value) ? null : r["TreePath"]),
 						SortOrder = (int)r["SortOrder"], 						
 						Status = (Boolean)r["Status"], 						
						Ids = MyModels.Encode((int)r["Id"], SecretId),						
					}).ToList();
            }


        }
        public static List<USMenu> GetUSMenuByGroups(int GroupId)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_menu",
                new string[] { "@flag", "@GroupId" },
                new object[] { "GetUSMenuByGroups", GroupId });
            return (from r in tabl.AsEnumerable()
                    select new USMenu
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
                        Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),                       
                        SortOrder = (int)r["SortOrder"],
                        Status = (Boolean)r["Status"],                        
                    }).ToList();
        }
        public static List<USMenu> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<USMenu>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new USMenu
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
 						IdParent = (int)r["IdParent"],
 						Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						SortOrder = (int)r["SortOrder"],
 						Status = (Boolean)r["Status"],
 						CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
 						CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
 						Deleted = (Boolean)r["Deleted"],						
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }

        }
       
        public static USMenu GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new USMenu
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
 						IdParent = (int)r["IdParent"],
 						Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						SortOrder = (int)r["SortOrder"],
 						Status = (Boolean)r["Status"],
 						CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
 						CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
 						Deleted = (Boolean)r["Deleted"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(USMenu dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
            new string[] { "@flag","@Id","@Title","@PathName","@IdParent","@Styles","@Description","@SortOrder","@Status","@CreatedBy","@CreatedDate","@ModifiedBy","@ModifiedDate","@Deleted" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.PathName,dto.IdParent,dto.Styles,dto.Description,dto.SortOrder,dto.Status,dto.CreatedBy,dto.CreatedDate,dto.ModifiedBy,dto.ModifiedDate,dto.Deleted });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(USMenu dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
