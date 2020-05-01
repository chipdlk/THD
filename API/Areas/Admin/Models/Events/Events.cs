using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Contacts;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Events
{
    public class Events
    {
		public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string Title { get; set; }
 		public string Description { get; set; }   
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		public int SortOrder { get; set; }
 		public string Image { get; set; }
        public DateTime DateExpired { get; set; }
        public String DateExpiredShow { get; set; }

        public int NumberRegist { get; set; }

    }
	
	public class EventsModel {
        public List<Events> ListItems { get; set; }       
        public SearchEvents SearchData { get; set; }
        public Events Item { get; set; }
        public PartialPagination Pagination { get; set; }
        public ContactsModel myContact { get; set; }

    }

    public class SearchEvents {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
    }
}
