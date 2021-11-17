using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class PeriodicalperiodValidation
    {
        public int IDTHDK { get; set; }
        [Required(ErrorMessage = "Nhập mã nhân viên và họ tên")]
        public int NhanSuID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Nhập trang bị BHLĐ theo vị trí")]
        public int DBID { get; set; }
        public string TenVTBHLD { get; set; }
        [Required(ErrorMessage = "Nhập phòng ban")]
        public int PhongBanID { get; set; }
        public string TenPhongBan { get; set; }
        [Required(ErrorMessage = "Nhập vị trí làm việc")]
        public int VTLVID { get; set; }
        public string TenVTLV { get; set; }
        [Required(ErrorMessage = "Nhập vị trí làm việc")]
        public DateTime NgayNV { get; set; }
    }
}