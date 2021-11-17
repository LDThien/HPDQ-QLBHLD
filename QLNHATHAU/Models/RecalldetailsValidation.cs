using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class RecalldetailsValidation
    {
        public int IDTHCT { get; set; }

        public int ThuHoiID { get; set; }
        public int BHLDID { get; set; }
        public string TenBHLD { get; set; }

        public int SoLuong { get; set; }

        public string Size { get; set; }

        public int TinhTrangID { get; set; }
        public string TenTinhTrang { get; set; }
    }
}