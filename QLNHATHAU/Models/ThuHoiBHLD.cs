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
    
    public partial class ThuHoiBHLD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThuHoiBHLD()
        {
            this.ThuHoiBHLDCTs = new HashSet<ThuHoiBHLDCT>();
        }
    
        public int IDThuHoi { get; set; }
        public Nullable<int> NhanSuID { get; set; }
        public Nullable<int> PhongBanID { get; set; }
        public Nullable<int> VTLVID { get; set; }
        public Nullable<System.DateTime> NgayTH { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThuHoiBHLDCT> ThuHoiBHLDCTs { get; set; }
    }
}
