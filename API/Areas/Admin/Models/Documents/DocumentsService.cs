using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Documents;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Documents
{
    public class DocumentsService
    {
        public static List<Documents> GetListPagination(SearchDocuments dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }

            string str_sql = "GetListPagination";

            if (dto.Status!=-1)
            {
                str_sql = "GetListPagination_Status";

            }

            
 

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@TypeId", "@LevelId", "@FieldId", "@CatId", "@StartDate", "@EndDate" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.TypeId, dto.LevelId, dto.FieldId, dto.CatId, StartDate, EndDate });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Documents
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Status = (Boolean)r["Status"],
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                            //EffectiveDate = (DateTime?)((r["EffectiveDate"] == System.DBNull.Value) ? null : r["EffectiveDate"]),
                            IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                            EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? null : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                            CatId = (int)r["CatId"],
                            TypeId = (int)r["TypeId"],
                            FieldId = (int)r["FieldId"],
                            LevelId = (int)r["LevelId"],
                            OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Ẩn", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Hiện", Value = "1" }));
            return ListItems;
        }

        /*
        public static List<Documents> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Documents
					{
						Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
 						Status = (Boolean)r["Status"], 						
 						IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
 						EffectiveDate = (DateTime)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now : r["EffectiveDate"]),
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                        EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                        CatId = (int)r["CatId"],
 						TypeId = (int)r["TypeId"],
                        FieldId = (int)r["FieldId"],
                        LevelId = (int)r["LevelId"],
                        OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),												
					}).ToList();
            }

        }*/

        public static Documents GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Documents
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                        Status = (Boolean)r["Status"],
                        IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                        EffectiveDate = (DateTime?)((r["EffectiveDate"] == System.DBNull.Value) ? null : r["EffectiveDate"]),
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                        EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ?  null: (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                        CatId = (int)r["CatId"],
                        TypeId = (int)r["TypeId"],
                        FieldId = (int)r["FieldId"],
                        LevelId = (int)r["LevelId"],
                        OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Documents dto)
        {
            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            DateTime IssuedDate = DateTime.ParseExact(dto.IssuedDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String EffectiveDate = null;
            if (dto.EffectiveDateShow != null)
                EffectiveDate = DateTime.ParseExact(dto.EffectiveDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString(); ;

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Title", "@Code", "@Link", "@Alias", "@Introtext", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@IssuedDate", "@EffectiveDate", "@CatId", "@TypeId", "@FieldId", "@LevelId", "@OrganizationName" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Code, dto.Link, dto.Alias, dto.Introtext, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, IssuedDate, EffectiveDate, dto.CatId, dto.TypeId, dto.FieldId, dto.LevelId, dto.OrganizationName });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
