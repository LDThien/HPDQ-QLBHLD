using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ApproveValidation
    {
        public int IDPD { get; set; }

        public int NguoiDungID { get; set; }
        public string TenDangNhap { get; set; }

        public int LoaiHSID { get; set; }
        public string TenLoai { get; set; }

        public int DBBHLDID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string TenVT { get; set; }
        public string TenBHLDTVT { get; set; }
        public string TenPB { get; set; }

        public string GhiChu { get; set; }
        public int CapPD { get; set; }

        public int TinhTrang { get; set; }
    }
}