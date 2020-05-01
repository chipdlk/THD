using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DuAn
{
    public class DuAn
    {
        public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(250, MinimumLength = 0, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Ten { get; set; }
        [StringLength(250, MinimumLength = 0, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        public string ChuNhiem { get; set; }
        public string MucTieu { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        public string LinhVuc { get; set; }
        public string PhuongPhap { get; set; }
        public string FileKetQua { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public string ThoiGianBatDauShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public DateTime ThoiGianKetThuc { get; set; }
        public string ThoiGianKetThucShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");

        [StringLength(250, MinimumLength = 0, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        public string KetQuaThucHien { get; set; }
        public string KinhPhi { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Loại không được để trống")]

        public int Loai { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Trạng thái không được để trống")]
        public int TrangThai { get; set; }

        public string Image { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Hiển thị không được để trống")]
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }


    }

    public class DuAnModel
    {
        public List<DuAn> ListItems { get; set; }
        public SearchDuAn SearchData { get; set; }
        public DuAn Item { get; set; }
        public PartialPagination Pagination { get; set; }
  
        public List<SelectListItem> ListItemsStatus { get; set; }
        public List<SelectListItem> ListItemsTrangThai { get; set; }
        public List<SelectListItem> ListItemsLoai { get; set; }
    }

    public class SearchDuAn
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
        public string Ten { get; set; }

        public int Loai { get; set; } = 0;
        public int TrangThai { get; set; } = 0;
   
        public string ShowStartDate { get; set; } = "01/01/" + DateTime.Now.ToString("yyyy");
        public string ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;

    }
}
