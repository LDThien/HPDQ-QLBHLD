using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class NguoiDungValidation
    {
        public int IDNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public int NhanVienID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string TenViTri { get; set; }
        public string TenPhongBan { get; set; }
        public int IDQuyen { get; set; }
        public string TenQuyen { get; set; }
        public int IsLock { get; set; }
    }
}