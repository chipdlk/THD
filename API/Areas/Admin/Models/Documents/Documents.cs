using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Documents
{
    public class Documents
    {
        public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }
        public string Alias { get; set; }
        [Display(Name = "Số ký hiệu")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Độ dài Số ký hiệu phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Số ký hiệu không được để trống")]
        public string Code { get; set; }
        public string Introtext { get; set; }
        public string FullText { get; set; }
        public string Link { get; set; }
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedDateShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public DateTime? EffectiveDate { get; set; }
        public string EffectiveDateShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public int CatId { get; set; }
        public int TypeId { get; set; }
        public int FieldId { get; set; }
        public int LevelId { get; set; }
        public string OrganizationName { get; set; }

    }

    public class DocumentsModel
    {
        public List<Documents> ListItems { get; set; }
        public SearchDocuments SearchData { get; set; }
        public Documents Item { get; set; }
        public PartialPagination Pagination { get; set; }
        public List<SelectListItem> ListDocumentsCategories { get; set; }
        public List<SelectListItem> ListDocumentsType { get; set; }
        public List<SelectListItem> ListDocumentsField { get; set; }
        public List<SelectListItem> ListDocumentsLevel { get; set; }
        public List<SelectListItem> ListItemsStatus { get; set; }
    }

    public class SearchDocuments
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public int CatId { get; set; }
        public int TypeId { get; set; }
        public int FieldId { get; set; }
        public int LevelId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime IssuedDateStart { get; set; }
        public DateTime IssuedDateEnd { get; set; } = DateTime.Now;
        public DateTime EffectiveDateStart { get; set; }
        public DateTime EffectiveDateEnd { get; set; } = DateTime.Now;
        public string ShowStartDate { get; set; } = "01/01/2000";
        public string ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;

    }
}
