//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNHATHAU.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ThuHoiBHLDCT
    {
        public int IDTHCT { get; set; }
        public Nullable<int> ThuHoiID { get; set; }
        public Nullable<int> BHLDID { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string Size { get; set; }
        public Nullable<int> TinhTrangID { get; set; }
    
        public virtual LoaiBHLD LoaiBHLD { get; set; }
        public virtual ThuHoiBHLD ThuHoiBHLD { get; set; }
    }
}
