using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DuAn;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DuAn
{
    public class DuAnService
    {
        public static List<DuAn> GetListPagination(SearchDuAn dto, string SecretId)
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
  
            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuAn",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@TrangThai", "@Loai", "@StatusSearch", "@StartDate", "@EndDate" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.TrangThai, dto.Loai,dto.Status, StartDate, EndDate });
            if (tabl == null)
            {
                return new List<DuAn>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DuAn
                        {
                            Id = (int)r["Id"],
                            Ten= (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                            ChuNhiem = (string)((r["ChuNhiem"] == System.DBNull.Value) ? null : r["ChuNhiem"]),
                            MucTieu = (string)((r["MucTieu"] == System.DBNull.Value) ? null : r["MucTieu"]),
                            LinhVuc = (string)((r["LinhVuc"] == System.DBNull.Value) ? null : r["LinhVuc"]),
                            PhuongPhap = (string)((r["PhuongPhap"] == System.DBNull.Value) ? null : r["PhuongPhap"]),
                            FileKetQua = (string)((r["FileKetQua"] == System.DBNull.Value) ? null : r["FileKetQua"]),                            
                            ThoiGianBatDau = (DateTime)((r["ThoiGianBatDau"] == System.DBNull.Value) ? DateTime.Now : r["ThoiGianBatDau"]),
                            ThoiGianKetThuc = (DateTime)((r["ThoiGianKetThuc"] == System.DBNull.Value) ? DateTime.Now : r["ThoiGianKetThuc"]),
                            KetQuaThucHien = (string)((r["KetQuaThucHien"] == System.DBNull.Value) ? null : r["KetQuaThucHien"]),
                            KinhPhi = (string)((r["KinhPhi"] == System.DBNull.Value) ? null : r["KinhPhi"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Loai = (int)r["Loai"],
                            TrangThai = (int)r["TrangThai"],
                            Status = (Boolean)r["Status"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"]

                        }).ToList();
            }


        }


        public static List<DuAn> GetListPaginationFront(SearchDuAn dto, string SecretId)
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

            string str_sql = "GetListPagination_Status";

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuAn",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@TrangThai", "@Loai", "@StatusSearch", "@StartDate", "@EndDate" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.TrangThai, dto.Loai, dto.Status, StartDate, EndDate });
            if (tabl == null)
            {
                return new List<DuAn>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DuAn
                        {
                            Id = (int)r["Id"],
                            Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                            ChuNhiem = (string)((r["ChuNhiem"] == System.DBNull.Value) ? null : r["ChuNhiem"]),
                            MucTieu = (string)((r["MucTieu"] == System.DBNull.Value) ? null : r["MucTieu"]),
                            LinhVuc = (string)((r["LinhVuc"] == System.DBNull.Value) ? null : r["LinhVuc"]),
                            PhuongPhap = (string)((r["PhuongPhap"] == System.DBNull.Value) ? null : r["PhuongPhap"]),
                            FileKetQua = (string)((r["FileKetQua"] == System.DBNull.Value) ? null : r["FileKetQua"]),
                            ThoiGianBatDau = (DateTime)((r["ThoiGianBatDau"] == System.DBNull.Value) ? DateTime.Now : r["ThoiGianBatDau"]),
                            ThoiGianKetThuc = (DateTime)((r["ThoiGianKetThuc"] == System.DBNull.Value) ? DateTime.Now : r["ThoiGianKetThuc"]),
                            KetQuaThucHien = (string)((r["KetQuaThucHien"] == System.DBNull.Value) ? null : r["KetQuaThucHien"]),
                            KinhPhi = (string)((r["KinhPhi"] == System.DBNull.Value) ? null : r["KinhPhi"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Loai = (int)r["Loai"],
                            TrangThai = (int)r["TrangThai"],
                            Status = (Boolean)r["Status"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"]

                        }).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Hiển thị ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Ẩn", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Hiện", Value = "1" }));
            return ListItems;
        }

        public static List<SelectListItem> GetListItemLoai()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Loại ---", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Đề tài KHKT", Value = "1" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Dự án", Value = "2" }));
            return ListItems;
        }


        public static List<SelectListItem> GetListItemTrangThai()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng thái ---", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Đang thực hiện", Value = "1" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Đã có kết quả", Value = "2" }));
            return ListItems;
        }

        

        public static DuAn GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuAn",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DuAn
                    {
                        Id = (int)r["Id"],
                        Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                        ChuNhiem = (string)((r["ChuNhiem"] == System.DBNull.Value) ? null : r["ChuNhiem"]),
                        MucTieu = (string)((r["MucTieu"] == System.DBNull.Value) ? null : r["MucTieu"]),
                        LinhVuc = (string)((r["LinhVuc"] == System.DBNull.Value) ? null : r["LinhVuc"]),
                        PhuongPhap = (string)((r["PhuongPhap"] == System.DBNull.Value) ? null : r["PhuongPhap"]),
                        FileKetQua = (string)((r["FileKetQua"] == System.DBNull.Value) ? null : r["FileKetQua"]),
                        ThoiGianBatDau = (DateTime)((r["ThoiGianBatDau"] == System.DBNull.Value) ? DateTime.Now : r["ThoiGianBatDau"]),
                        ThoiGianBatDauShow = (string)((r["ThoiGianBatDau"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["ThoiGianBatDau"]).ToString("dd/MM/yyyy")),
                        ThoiGianKetThuc = (DateTime)((r["ThoiGianKetThuc"] == System.DBNull.Value) ? DateTime.Now : r["ThoiGianKetThuc"]),
                        ThoiGianKetThucShow = (string)((r["ThoiGianKetThuc"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["ThoiGianKetThuc"]).ToString("dd/MM/yyyy")),
                        KetQuaThucHien = (string)((r["KetQuaThucHien"] == System.DBNull.Value) ? null : r["KetQuaThucHien"]),
                        KinhPhi = (string)((r["KinhPhi"] == System.DBNull.Value) ? null : r["KinhPhi"]),
                        Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Loai = (int)r["Loai"],
                        TrangThai = (int)r["TrangThai"],
                        Status = (Boolean)r["Status"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId)
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DuAn dto)
        {
          
            DateTime NgayBD = DateTime.ParseExact(dto.ThoiGianBatDauShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime NgayKT = DateTime.ParseExact(dto.ThoiGianKetThucShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuAn",
            new string[] { "@flag", "@Id", "@Ten", "@ChuNhiem", "@MucTieu", "@LinhVuc", "@PhuongPhap", "@FileKetQua", "@Status", "@ThoiGianBatDau", "@ThoiGianKetThuc", "@KetQuaThucHien", "@KinhPhi", "@Loai", "@Image", "@TrangThai", "@CreatedBy", "@ModifiedBy", },
           new object[] { "SaveItem", dto.Id, dto.Ten, dto.ChuNhiem, dto.MucTieu, dto.LinhVuc, dto.PhuongPhap, dto.FileKetQua, dto.Status, NgayBD, NgayKT, dto.KetQuaThucHien, dto.KinhPhi, dto.Loai, dto.Image, dto.TrangThai, dto.CreatedBy, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DuAn dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuAn",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DuAn dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuAn",
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
