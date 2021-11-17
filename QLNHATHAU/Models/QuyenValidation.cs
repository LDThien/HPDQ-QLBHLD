using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class QuyenValidation
    {
        [Required(ErrorMessage = "Nhập ID Quyền.")]
        public int IDQuyen { get; set; }

        [Required(ErrorMessage = "Nhập tên quyền.")]
        public string TenQuyen { get; set; }
    }
}