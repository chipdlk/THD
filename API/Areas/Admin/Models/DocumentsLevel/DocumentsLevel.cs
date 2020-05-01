using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.DocumentsLevel
{
    public class DocumentsLevel
    {
		public string Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string Title { get; set; }
 		public string Alias { get; set; }
 		
    }
	
	public class DocumentsLevelModel {
        public List<DocumentsLevel> ListItems { get; set; }       
        public SearchDocumentsLevel SearchData { get; set; }
        public DocumentsLevel Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }

    public class SearchDocumentsLevel {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string Keyword { get; set; }
    }
}
