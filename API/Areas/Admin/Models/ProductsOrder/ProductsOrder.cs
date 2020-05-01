using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ProductsOrder
{
    public class ProductsOrder
    {
        public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; } = 0;
        [Display(Name = "Họ tên")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string Name { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Điện thoại không được để trống")]
        public string Phone { get; set; }
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        public string Email { get; set; }
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        public string Address { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public Boolean Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ProductsOrderModel
    {
        public List<ProductsOrder> ListItems { get; set; }
        public SearchProductsOrder SearchData { get; set; }
        public ProductsOrder Item { get; set; }
        public PartialPagination Pagination { get; set; }
        public List<ProductsOrderDetail.ProductsOrderDetail> ListDetail{ get; set; }

    }

    public class SearchProductsOrder
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }      
        public string Keyword { get; set; }
        public int Status { get; set; } = -1;
    }
}
