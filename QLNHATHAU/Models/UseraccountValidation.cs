using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class UseraccountValidation
    {
        [Required(ErrorMessage = "Nhập ID người dùng")]
        public int IDNguoiDung { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu")]
        public string MatKhau { get; set; }

        public int NhanVienID { get; set; }

        public int IDQuyen { get; set; }

        public int IsLock { get; set; }

        public string NhapLaiMatKhau { get; set; }

        public string MatKhauCu { get; set; }

    }
}