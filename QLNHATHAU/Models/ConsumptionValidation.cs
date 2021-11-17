using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ConsumptionValidation
    {
        public int IDDBTH { get; set; }

        [Required(ErrorMessage = "Nhập mã nhân viên và họ tên")]
        public int NhanSuID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Nhập trang bị BHLĐ theo vị trí")]
        public int DBTHID { get; set; }
        public string TenVTBHLD { get; set; }

        [Required(ErrorMessage = "Nhập phòng ban")]
        public int PhongBanID { get; set; }
        public string TenPhongBan { get; set; }

        [Required(ErrorMessage = "Nhập vị trí làm việc")]
        public int VTLVID { get; set; }
        public string TenVTLV { get; set; }

        [Required(ErrorMessage = "Nhập ngày")]

        public DateTime NgayXuat { get; set; }

        public int TinhTrangID { get; set; }
        public string TenTT { get; set; }

        public DateTime? Begind { get; set; }

        //Ngày kết thúc
        public DateTime? Endd { get; set; }
        public int PheDuyetID { get; set; }
    }
}