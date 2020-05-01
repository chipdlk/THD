using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using API.Areas.Admin.Models.Ablums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesAblums
{
    public class CategoriesAblums
    {
		public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]

        public string Title { get; set; }
        public string Alias { get; set; }
 		public string Description { get; set; }
 		public string Images { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Featured { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public int ParentId { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
        public List<Ablums.Ablums> ListItemsAblums { get; set; }
 		
    }
	
	public class CategoriesAblumsModel {
        public List<CategoriesAblums> ListItems { get; set; }        
        public List<Ablums.Ablums> ListItemsAlbums { get; set; }
        public List<SelectListItem> ListItemsParents { get; set; }
        public SearchCategoriesAblums SearchData { get; set; }
        public CategoriesAblums Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }

    public class SearchCategoriesAblums {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
    }
}
