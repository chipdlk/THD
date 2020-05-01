using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ProductsOrderDetail
{
    public class ProductsOrderDetail
    {
        public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int ProductOrderId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public Decimal? Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public Boolean Deleted { get; set; }
    }
    public class ProductsOrderUpdate
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }



    public class ProductsOrderDetailModel
    {
        public List<ProductsOrderDetail> ListItems { get; set; }
        public SearchProductsOrderDetail SearchData { get; set; }
        public ProductsOrderDetail Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }

    public class SearchProductsOrderDetail
    {        public int ProductOrderId { get; set; }
        public int ProductId { get; set; }
    }
}
