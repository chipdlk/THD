using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.DMCoQuan
{
    public class DMCoQuanService
    {
        public static List<DMCoQuan> GetListPagination(SearchDMCoQuan dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CategoryId" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CategoryId });
            if (tabl == null)
            {
                return new List<DMCoQuan>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMCoQuan
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Title1 = (string)((r["Title1"] == System.DBNull.Value) ? null : r["Title1"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Status = (bool)r["Status"],
                            CategoryId = (int)((r["CategoryId"] == System.DBNull.Value) ? 0 : r["CategoryId"]),
                            ParentId = (int)((r["ParentId"] == System.DBNull.Value) ? 0 : r["ParentId"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"]
                        }).ToList();
            }           
        }

        public static DataTable GetList(Boolean CHON = true)
        {
            decimal chon = Convert.ToDecimal(CHON);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@sCHON" }, new object[] { "GetList", chon });
            return tabl;

        }
        public static List<SelectListItem> GetListByLoaiCoQuan(int CategoryId, int Def = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@Selected", "@CategoryId" }, new object[] { "GetListByLoaiCoQuan", 1, CategoryId });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();
            if (Def==0)
            {
                ListItems.Insert(0, (new SelectListItem { Text = "Chọn Cơ Quan cha", Value = "0" }));
            }
            
            
            return ListItems;
        }

        public static List<SelectListItem> GetListLayout()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>(){};
            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Giao diện", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Giao diện Khoa", Value = "1" }));         
            return ListItems;
        }

        public static DMCoQuan GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMCoQuan
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Code = (string)r["Code"],
                        Description = (object)r["Description"],
                        Status = (bool)r["Status"],
                        CategoryId = (int)r["CategoryId"],
                        ParentId = (int)r["ParentId"],                      
                        IdLayout = (int)r["IdLayout"],
                        Title_1 = (string)((r["Title_1"] == System.DBNull.Value) ? "" : r["Title_1"]),
                        Title_2 = (string)((r["Title_2"] == System.DBNull.Value) ? "" : r["Title_2"]),
                        Title_3 = (string)((r["Title_3"] == System.DBNull.Value) ? "" : r["Title_3"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static DMCoQuan GetItemByCode(string Code)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Code" }, new object[] { "GetItemByCode", Code.Trim() });
            return (from r in tabl.AsEnumerable()
                    select new DMCoQuan
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Code = (string)r["Code"],
                        Description = (object)r["Description"],
                        Status = (bool)r["Status"],
                        CategoryId = (int)r["CategoryId"],
                        ParentId = (int)r["ParentId"],
                        IdLayout = (int)r["IdLayout"],
                        Title_1 = (string)((r["Title_1"] == System.DBNull.Value) ? "" : r["Title_1"]),
                        Title_2 = (string)((r["Title_2"] == System.DBNull.Value) ? "" : r["Title_2"]),
                        Title_3 = (string)((r["Title_3"] == System.DBNull.Value) ? "" : r["Title_3"])                        
                    }).FirstOrDefault();
        }
        public static dynamic SaveItem(DMCoQuan dto)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Title", "@Code", "@Id", "@Description", "@Status", "@IdLayout", "@Title_1", "@Title_2", "@Title_3", "@CreatedBy", "@ModifiedBy", "@CategoryId", "ParentId" },
            new object[] { "SaveItem", dto.Title, dto.Code, dto.Id, dto.Description, dto.Status,dto.IdLayout,dto.Title_1,dto.Title_2,dto.Title_3, dto.CreatedBy, dto.ModifiedBy, dto.CategoryId, dto.ParentId });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DMCoQuan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }
        public static dynamic UpdateStatus(DMCoQuan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
    }
}
