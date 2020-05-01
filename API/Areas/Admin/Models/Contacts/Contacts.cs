using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.Contacts
{
    public class Contacts
    {
		public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
       
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }
 		public Boolean Status { get; set; }
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Fullname { get; set; }
        [StringLength(500, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Introtext { get; set; }
 		public string Description { get; set; }
 		public string Phone { get; set; }
 		public string Email { get; set; }
 		public string Address { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime CreatedDate { get; set; }
        public String CreatedDateShow { get; set; }
        public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
        public int Type { get; set; }
 		
        public int EventId { get; set; }
    }
	
	public class ContactsModel {
        public List<Contacts> ListItems { get; set; }       
        public SearchContacts SearchData { get; set; }
        public Contacts Item { get; set; }
        public PartialPagination Pagination { get; set; }
        public int eID { get; set; }
    }

    public class SearchContacts {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
        public int Type { get; set; }
    }
}
