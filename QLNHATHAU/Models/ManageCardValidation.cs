using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace QLNHATHAU.Models
{
    public class ManageCardValidation
    {
        public int ID { get; set; }
        public int CardID { get; set; }

        [Required(ErrorMessage = "Nhập số card")]

        [Remote("IsAlreadySbb", "ManageCard", HttpMethod = "POST", ErrorMessage = "Mã số đã tồn tại.")]

        public string MSCard { get; set; }
        public int NhanVienNTID { get; set; }
        public string MaNhanVienNT { get; set; }
        public string TenNVNT { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayHetHan { get; set; }
        public int NhaThauID { get; set; }
        public string NhaThau { get; set; }
        public bool TTThe { get; set; }
        public int TSVP { get; set; }
    }
}