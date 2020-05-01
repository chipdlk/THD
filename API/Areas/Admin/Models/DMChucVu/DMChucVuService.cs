using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMChucVu;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMChucVu
{
    public class DMChucVuService
    {
        public static List<DMChucVu> GetListPagination(SearchDMChucVu dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<DMChucVu>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMChucVu
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            Leader = (bool)r["Leader"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static DataTable GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return tabl;

        }

        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Chức Vụ ---", Value = "0" }));
            return ListItems;

        }

        public static DMChucVu GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMChucVu
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Leader = (bool)r["Leader"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DMChucVu dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag","@Id", "@Title", "@Status", "@Description", "@Leader", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, dto.Description, dto.Leader,dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DMChucVu dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(DMChucVu dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
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
