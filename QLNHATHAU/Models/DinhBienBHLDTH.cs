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
    
    public partial class DinhBienBHLDTH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DinhBienBHLDTH()
        {
            this.DinhBienBHLDTHCTs = new HashSet<DinhBienBHLDTHCT>();
            this.DinhBienTHs = new HashSet<DinhBienTH>();
        }
    
        public int IDDBTH { get; set; }
        public string TenVTBHLD { get; set; }
        public Nullable<int> VTLVID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DinhBienBHLDTHCT> DinhBienBHLDTHCTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DinhBienTH> DinhBienTHs { get; set; }
    }
}