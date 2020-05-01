using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Videos
{
    public class Videos
    {
		public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string Title { get; set; }
 		public string TitleType { get; set; }
 		public string Description { get; set; }
 		public string Link { get; set; }
 		public int? CatId { get; set; }
 		public string Image { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Featured { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		public int IdType { get; set; }
 		
    }
	
	public class VideosModel {
        public List<Videos> ListItems { get; set; }       
        public List<SelectListItem> ListItemsType { get; set; }       
        public SearchVideos SearchData { get; set; }
        public Videos Item { get; set; }
        public PartialPagination Pagination { get; set; }
        public int IdType { get; set; }
    }

    public class SearchVideos {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdType { get; set; }
        public string Keyword { get; set; }
    }
}
