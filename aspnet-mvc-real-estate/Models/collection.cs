//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace aspnet_mvc_real_estate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class collection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public collection()
        {
            this.collections_galery = new HashSet<collections_galery>();
            this.products = new HashSet<product>();
        }
    
        public int collections_id { get; set; }
        public string collections_name { get; set; }
        public string collections_slug { get; set; }
        public string overview { get; set; }
        public string video { get; set; }
        public string introduction { get; set; }
        public string information { get; set; }
        public string location { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<collections_galery> collections_galery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<product> products { get; set; }
    }
}
