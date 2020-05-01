using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.DMPhuongXa
{
    public class DMPhuongXa
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string Ma { get; set; }
        public string MaQuanHuyen { get; set; }
        public int IdQuanHuyen { get; set; }
        public bool TrangThai { get; set; }
        public string MoTa { get; set; }
        public string Ids { get; set; }
        public string TenQuanHuyen { get; set; }
        public string TenTinhThanh { get; set; }
        public int TotalRows { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }

    }

    public class DMPhuongXaModel
    {
        public List<DMPhuongXa> ListItems { get; set; }
        public List<SelectListItem> ListItemsTinhThanh { get; set; }
        public List<SelectListItem> ListItemsQuanHuyen { get; set; }
        public SearchDMPhuongXa SearchData { get; set; }
        public DMPhuongXa Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }
    public class SearchDMPhuongXa
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdQuanHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public string Keyword { get; set; }
    }
}
