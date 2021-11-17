using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class PermissionValidation
    {
        [Required(ErrorMessage = "Nhập ID phân quyền.")]
        public int IDPhanQuyen { get; set; }

        [Required(ErrorMessage = "Nhập ID người dùng.")]
        public int NguoiDungID { get; set; }

        [Required(ErrorMessage = "Nhập ID quyền.")]
        public int QuyenID { get; set; }

        public string TenDangNhap { get; set; }
    }
}