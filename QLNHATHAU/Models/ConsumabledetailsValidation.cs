using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ConsumabledetailsValidation
    {
        public int IDBHLDTH { get; set; }

        public int DBTHID { get; set; }
        public string TenBHLDTVT { get; set; }
        [Required(ErrorMessage = "Nhập trạng bị bảo hộ lao động")]
        public int BHLDID { get; set; }
        public string TenBHLD { get; set; }
        [Required(ErrorMessage = "Nhập số lượng")]
        public int Soluong { get; set; }
    }
}