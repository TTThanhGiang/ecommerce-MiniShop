//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThanhGiang_WebCuoiKi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbHOADON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbHOADON()
        {
            this.tbCHITIETHOADONs = new HashSet<tbCHITIETHOADON>();
        }
    
        public int MAHOADON { get; set; }
        public Nullable<int> MAKHACHHANG { get; set; }
        public Nullable<System.DateTime> NGAYTAO { get; set; }
        public string TRANGTHAI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbCHITIETHOADON> tbCHITIETHOADONs { get; set; }
        public virtual tbKHACHHANG tbKHACHHANG { get; set; }
    }
}
