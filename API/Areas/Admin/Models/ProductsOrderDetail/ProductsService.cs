using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.ProductsOrder;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.ProductsOrder
{
    public class ProductsOrderService
    {
        public static List<ProductsOrder> GetListPagination(SearchProductsOrder dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrder",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@Status" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.Status });
            if (tabl == null)
            {
                return new List<ProductsOrder>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ProductsOrder
                        {
                            Id = (int)r["Id"],
                            Name = (string)((r["Name"] == System.DBNull.Value) ? null : r["Name"]),
                            Phone= (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
                            Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                            Status = (int)r["Status"],
                            CreatedDate= (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }


        }
        

        public static ProductsOrder GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrder",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new ProductsOrder
                    {
                        Id = (int)r["Id"],
                        Name = (string)((r["Name"] == System.DBNull.Value) ? null : r["Name"]),
                        Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                        Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
                        Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                        Status = (int)r["Status"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId)
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(ProductsOrder dto)
        {            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrder",
            new string[] { "@flag", "@Id", "@Name", "@Phone", "@Email", "@Address", "@Description", "@Status" },
            new object[] { "SaveItem", dto.Id, dto.Name,dto.Phone,dto.Email,dto.Address,dto.Description,dto.Status});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(ProductsOrder dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrder",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(ProductsOrder dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrder",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static List<SelectListItem> GetListItemsStatusBack()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Chưa xử lý", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Đang xử lý", Value = "1" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Đã xử lý", Value = "2" }));
            ListItems.Insert(3, (new SelectListItem { Text = "Đã hủy", Value = "3" }));
            return ListItems;
        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Tất cả", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Chưa xử lý", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Đang xử lý", Value = "1" }));
            ListItems.Insert(3, (new SelectListItem { Text = "Đã xử lý", Value = "2" }));
            ListItems.Insert(4, (new SelectListItem { Text = "Đã hủy", Value = "3" }));
            return ListItems;
        }


        public static string GetTextStatus(int value)
        {
            return value == 0 ? "Chưa xử lý"
                : value == 1 ? "Đang xử lý"
                : value == 2 ? "Đã xử lý"
                : value == 3 ? "Đã hủy"
                : "";
        }

    }
}
