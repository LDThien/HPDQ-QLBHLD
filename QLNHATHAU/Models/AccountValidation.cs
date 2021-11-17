using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class AccountValidation
    {
        public int ID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string HoTenKhongDau { get; set; }

        public DateTime NgaySinh { get; set; }

        public string DiaChi { get; set; }

        public string DienThoai { get; set; }

        public DateTime NgayVaoLam { get; set; }

        public int IDPhongBan { get; set; }
        public string TenPhongBan { get; set; }

        public int IDTinhTrangLV { get; set; }

        public int IDViTri { get; set; }
        public string TenViTri { get; set; }

        public int CMND { get; set; }

        public int IDQuyen { get; set; }

        public string TenQuyen { get; set; }
    }
}