using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class PeriodicdetailsValidation
    {
        public int IDBHLDDK { get; set; }
      
        public int DBDKID { get; set; }
        public string TenBHLDTVT { get; set; }
     
        public int BHLDID { get; set; }
        public string TenBHLD { get; set; }
       
        public int Soluong { get; set; }
        public string Size { get; set; }

        public int SLCL { get; set; }
    }
}