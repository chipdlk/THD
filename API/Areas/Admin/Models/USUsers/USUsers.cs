using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.USUsers
{
    public class USUsers
    {
        public int Id { get; set; }
        [Display(Name = "UserName")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài Tài khoản lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }
        public string UserCode { get; set; }
        [Display(Name = "Email")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài Email lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }       
        public string Fax { get; set; }        
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int Birthday { get; set; }
        public string BirthdayShow { get; set; }
        public byte Gender { get; set; } = 1;
        public int IdGroup { get; set; }
        public string GroupsTitle { get; set; }
        public int TotalRows { get; set; }
        public byte Status { get; set; } = 1;
        public int CreatedBy { get; set; }
        public int IdCoQuan { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean Deleted { get; set; } = false;
        public string Ids { get; set; }
        public string TenCoQuan { get; set; }
        public string Specialize { get; set; }
        public string TenChucVu { get; set; }
        public int IdRegency { get; set; }
    }

    public class USInformation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string UserCode { get; set; }
        public string Avatar { get; set; }
        public string Fax { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }        
        public int Birthday { get; set; }
        public byte Gender { get; set; } = 1;
//        public MyEntity.MyDateTime BirthdayShow { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public int IdGroup { get; set; }
        public int IdCoQuan  { get; set; }
        public XSRF XSRFInfo { get; set; }
        public UserToken UserToken { get; set; }
        public dynamic ListRoutes { get; set; }
        public dynamic MenuInfo { get; set; }
    }

    public class USUsersLog
    {
        public string Browser { get; set; }
        public string Platform { get; set; }
        public int IdUser { get; set; }
        public string LoginInfo { get; set; }
        public string Ip { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
        public int CreatedDate { get; set; }        
    }
    public class AccountLogin
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int IdTypeSocial { get; set; }

    }
    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
        public int Id { get; set; }
    }

    public class SearchUSUsers
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public string Keyword { get; set; }
    }

    

    public class UserToken
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Email { get; set; }     
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string FullName { get; set; }
        public int Birthday { get; set; }
        public byte Gender { get; set; } = 1;
        public int IdGroup { get; set; }
        public int IdCoQuan { get; set; }
        public string GroupsName { get; set; }
        public string Avatar { get; set; }
        public string TenChucVu { get; set; }
        public int IdRegency { get; set; }
       
    }

    public class USUsersModel
    {
        public List<USUsers> ListItems { get; set; }
        public List<SelectListItem> ListItemsGroups { get; set; }
        public List<SelectListItem> ListItemsStatus { get; set; }
        public List<SelectListItem> ListDMCoQuan { get; set; }
        public List<SelectListItem> ListDMChucVu { get; set; }
        public SearchUSUsers SearchData { get; set; }
        public USUsers Item { get; set; }
        public PartialPagination Pagination { get; set; }
    }

    public class XSRF
    {
        public string X_XSRF_TOKEN { get; set; }
        public string DateTime { get; set; }
        public string Token { get; set; }
    }
}
