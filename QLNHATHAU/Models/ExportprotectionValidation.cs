using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ExportprotectionValidation
    {
        public int IDBHLDX { get; set; }

       
        public int BDNVID { get; set; }
        public string TenBHLDTVT { get; set; }
      
        public int BHLDID { get; set; }
        public string TenBHLD { get; set; }

        public int Soluong { get; set; }
    
        public string Size { get; set; }

        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string TenVTBHLD { get; set; }
        public string TenPhongBan { get; set; }
        public string TenVTLV { get; set; }

    }
}