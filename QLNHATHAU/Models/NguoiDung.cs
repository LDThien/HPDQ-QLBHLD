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
    
    public partial class NguoiDung
    {
        public int IDNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public Nullable<int> NhanVienID { get; set; }
        public Nullable<int> IDQuyen { get; set; }
        public Nullable<int> IsLock { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
    }
}
