using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Reviews;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Reviews
{
    public class ReviewsService
    {
        public static List<Reviews> GetListPagination(SearchReviews dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<Reviews>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Reviews
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),                            
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? 0 : r["Featured"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Start = (int)r["Start"],                            
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            DisplayOder = (int)r["DisplayOder"]
                        }).ToList();
            }


        }
        public static List<Reviews> GetListFeatured(int Limit=5,int Id=0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
                new string[] { "@flag", "@Limit","@Id" }, new object[] { "GetListFeatured",Limit,Id });
            if (tabl == null)
            {
                return new List<Reviews>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Reviews
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),                          
                            ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),                           
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Start = (int)r["Start"],
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            DisplayOder = (int)r["DisplayOder"]
                        }).ToList();
            }

        }


        
        public static List<Reviews> GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Reviews>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Reviews
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),                           
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? 0 : r["Featured"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Start = (int)r["Start"],
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            DisplayOder = (int)r["DisplayOder"]
                        }).ToList();
            }

        }

        public static List<SelectListItem> GetListStart()
        {

            List<SelectListItem> ListItems = new List<SelectListItem>();
            for (int i = 0; i < 6; i++) {
                ListItems.Insert(i, (new SelectListItem { Text = i.ToString(), Value =i.ToString() }));
            }                        
            return ListItems;
        }

        public static Reviews GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Reviews
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),
                        ReviewDateShow = (string)((r["ReviewDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["ReviewDate"]).ToString("dd/MM/yyyy")),
                        Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? 0 : r["Featured"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        Start = (int)r["Start"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        DisplayOder = (int)r["DisplayOder"]
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Reviews dto)
        {
            DateTime ReviewsDate = DateTime.ParseExact(dto.ReviewDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@Title", "@Description", "@Status", "@CreatedBy", "@ModifiedBy", "@Introtext", "@Start", "@FullName", "@ReviewDate", "@Image", "@DisplayOder", @"Featured" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Description, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.Introtext, dto.Start, dto.FullName, ReviewsDate, dto.Image,dto.DisplayOder,dto.Featured });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Reviews dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Reviews dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(Reviews dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        
    }
}
