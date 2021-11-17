using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ConsumptionperiodDetaiValidation
    {
        public int IDTHTHCT { get; set; }

        public int THTHID { get; set; }
        [Required(ErrorMessage = "Nhập trạng bị bảo hộ lao động")]
        public int BHLDID { get; set; }
        public string TenBHLD { get; set; }
        [Required(ErrorMessage = "Nhập thời hạn")]
        public int ThoiHan { get; set; }
        [Required(ErrorMessage = "Nhập ngày")]
        public DateTime NgayNV { get; set; }
    }
}