using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Events;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Events
{
    public class EventsService
    {
        public static List<Events> GetListPagination(SearchEvents dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<Events>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Events
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]), 				
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 	
 						SortOrder = (int)r["SortOrder"],
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),			
                        DateExpired=(DateTime)r["DateExpired"],
                        DateExpiredShow = (string)((DateTime)r["DateExpired"]).ToString("dd/MM/yyyy"),
                        NumberRegist = (int)r["NumberRegist"]
                    }).ToList();
            }


        }



        public static List<Events> GetListPaginationHome(SearchEvents dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPaginationHome", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<Events>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Events
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            SortOrder = (int)r["SortOrder"],
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            DateExpired = (DateTime)r["DateExpired"],
                            DateExpiredShow = (string)((DateTime)r["DateExpired"]).ToString("dd/MM/yyyy"),
                            NumberRegist = (int)r["NumberRegist"],
                        }).ToList();
            }

        }





        public static List<Events> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Events>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Events
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),                       
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 					
 					 	SortOrder = (int)r["SortOrder"], 						
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),						
						TotalRows = (int)r["TotalRows"],
                        DateExpired = (DateTime)r["DateExpired"],
                        DateExpiredShow = (string)((DateTime)r["DateExpired"]).ToString("dd/MM/yyyy")
                    }).ToList();
            }

        }

        public static Events GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Events
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),   
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 		
 						SortOrder = (int)r["SortOrder"],
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        DateExpired = (DateTime)r["DateExpired"],
                        DateExpiredShow = (string)((DateTime)r["DateExpired"]).ToString("dd/MM/yyyy"),
                        NumberRegist=(int)r["NumberRegist"],
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Events dto)
        {
            DateTime NgayDang = DateTime.ParseExact(dto.DateExpired.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
            new string[] { "@flag","@Id","@Title","@Description","@Status","@CreatedBy","@ModifiedBy","@SortOrder","@Image","@DateExpired"  },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Description,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.SortOrder,dto.Image,NgayDang});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Events dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Events dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Events",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
