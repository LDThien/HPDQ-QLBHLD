using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ApproveTHValidation
    {
        public int IDPDTH { get; set; }

        public int NguoiDungID { get; set; }
        public string TenDangNhap { get; set; }

        public int LoaiHSID { get; set; }
        public string TenLoai { get; set; }

        public int BHLDTHID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string TenVT { get; set; }
        public string TenBHLDTVT { get; set; }
        public string TenPB { get; set; }

        public string GhiChu { get; set; }

        public int TinhTrang { get; set; }
    }
}