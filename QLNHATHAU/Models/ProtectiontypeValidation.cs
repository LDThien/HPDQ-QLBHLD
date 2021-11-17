using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ProtectiontypeValidation
    {
        public int IDBHLD { get; set; }
        [Required(ErrorMessage = "Nhập trang bị bảo hộ lao động")]
        public string TenBHLD { get; set; }
    }
}