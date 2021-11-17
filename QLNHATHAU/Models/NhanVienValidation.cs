using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit;
using Xunit.Sdk;

namespace QLNHATHAU.Models
{
    public class NhanVienValidation
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


        public int IDTinhTrangLV { get; set; }

        public int IDViTri { get; set; }

        public string CMND { get; set; }
    }
}