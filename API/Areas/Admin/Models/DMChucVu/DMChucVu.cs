using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.DMChucVu
{
    public class DMChucVu
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên chức vụ không được để trống")]
        public string Title { get; set; }
        public string Description { get; set; }        
        public Boolean Status { get; set; }
        public Boolean Leader { get; set; }
        public string Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }

    public class DMChucVuModel {
        public List<DMChucVu> ListItems { get; set; }       
        public SearchDMChucVu SearchData { get; set; }
        public DMChucVu Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }
    public class SearchDMChucVu {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
    }
}
