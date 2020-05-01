using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.ProductsOrderDetail;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.ProductsOrderDetail
{
    public class ProductsOrderDetailService
    {
        public static List<ProductsOrderDetail> GetListPagination(SearchProductsOrderDetail dto, string SecretId)
        {
            
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrderDetail",
                new string[] { "@flag", "@ProductOrderId" },
                new object[] { "GetListByProductsOrderDetailId", dto.ProductOrderId});
            if (tabl == null)
            {
                return new List<ProductsOrderDetail>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ProductsOrderDetail
                        {
                            Id = (int)r["Id"],
                            ProductId = (int)r["ProductId"],
                            Name = (string)((r["Name"] == System.DBNull.Value) ? null : r["Name"]),
                            Price=(decimal)((r["Price"] == System.DBNull.Value) ? 0 : r["Price"]),
                            Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? null : r["Quantity"]),
                            Unit = (string)((r["Unit"] == System.DBNull.Value) ? null : r["Unit"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }


        }
        

        public static ProductsOrderDetail GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrderDetail",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new ProductsOrderDetail
                    {
                        Id = (int)r["Id"],
                        ProductId = (int)r["ProductId"],
                        Name = (string)((r["Name"] == System.DBNull.Value) ? null : r["Name"]),
                        Price = (decimal)((r["Price"] == System.DBNull.Value) ? 0 : r["Price"]),
                        Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? null : r["Quantity"]),
                        Unit = (string)((r["Unit"] == System.DBNull.Value) ? null : r["Unit"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(ProductsOrderDetail dto)
        {            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrderDetail",
            new string[] { "@flag", "@Id", "@ProductOrderId", "@ProductId","@Name", "@Price", "@Quantity", "@Unit"},
            new object[] { "SaveItem", dto.Id,dto.ProductOrderId,dto.ProductId,dto.Name,dto.Price,dto.Quantity,dto.Unit});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(ProductsOrderDetail dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ProductsOrderDetail",
            new string[] { "@flag", "@Id"},
            new object[] { "DeleteItem", dto.Id });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

      
        
    }
}
