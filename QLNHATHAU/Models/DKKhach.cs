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
    
    public partial class DKKhach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DKKhach()
        {
            this.DKKhachCTs = new HashSet<DKKhachCT>();
        }
    
        public int IDDangKyKH { get; set; }
        public Nullable<int> NhaThauID { get; set; }
        public Nullable<int> PhongBanID { get; set; }
        public Nullable<int> LoaiKhachID { get; set; }
        public Nullable<int> CongID { get; set; }
        public string HKTT { get; set; }
        public string NguoiDaiDien { get; set; }
        public string BienSo { get; set; }
        public string PhuongTien { get; set; }
        public Nullable<System.DateTime> NgayBL { get; set; }
        public string Ghi_Chu { get; set; }
        public Nullable<int> TinhTrang { get; set; }
    
        public virtual Cong Cong { get; set; }
        public virtual LoaiKhach LoaiKhach { get; set; }
        public virtual NhaThau NhaThau { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DKKhachCT> DKKhachCTs { get; set; }
    }
}
