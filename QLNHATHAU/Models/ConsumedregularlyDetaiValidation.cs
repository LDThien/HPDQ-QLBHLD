using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ConsumedregularlyDetaiValidation
    {
        public int IDDBTHCT { get; set; }

        public int BHLDID { get; set; }
        public string TenBHLD { get; set; }

        [Required(ErrorMessage = "Nhập trang bị bảo hộ lao động")]
        public int DBTHID { get; set; }
        public string TenVTBHLD { get; set; }

        [Required(ErrorMessage = "Nhập đơn vị tính")]
        public string DVT { get; set; }

        [Required(ErrorMessage = "Nhập số lượng")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Nhập thời hạn")]
        public int ThoiHan { get; set; }

        [Required(ErrorMessage = "Nhập ghi chú")]
        public string GhiChu { get; set; }
    }
}