using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ProtectionmarginValidation
    {
        public int IDDB { get; set; }
        [Required(ErrorMessage = "Nhập trạng bị BHLĐ theo vị trí")]
        public string TenVTBHLD { get; set; }

        [Required(ErrorMessage = "Nhập vị trí làm việc")]
        public int VTLVID { get; set; }
        public string TenVTLV { get; set; }
    }
}