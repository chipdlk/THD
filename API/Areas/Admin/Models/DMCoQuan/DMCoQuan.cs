using API.Areas.Admin.Models.Partial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace API.Areas.Admin.Models.DMCoQuan
{
    public class DMCoQuan
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên cơ quan không được để trống")]


        public string Title { get; set; }
        public string Title1 { get; set; }
        public string Code { get; set; }
        public object Description { get; set; }        
        public Boolean Status { get; set; }       
        public string Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean Deleted { get; set; }
        public int ParentId { get; set; }
        public int CategoryId { get; set; }
        public int Selected { get; set; }
        public string TitleCategory { get; set; }
        public int IdLayout { get; set; } = 0;
        public string Title_1 { get; set; }
        public string Title_2 { get; set; }
        public string Title_3 { get; set; }

    }
    public class DMCoQuanModel
    {
        public List<DMCoQuan> ListItems { get; set; }
        public List<SelectListItem> ListItemsCoQuan { get; set; }
        public List<SelectListItem> ListItemsLoaiCoQuan { get; set; }
        public List<SelectListItem> ListItemsLayout { get; set; }
        public SearchDMCoQuan SearchData { get; set; }
        public DMCoQuan Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }
    public class SearchDMCoQuan
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
    }
}
