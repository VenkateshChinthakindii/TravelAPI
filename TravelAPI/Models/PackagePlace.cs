//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TravelAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PackagePlace
    {
        public int id { get; set; }
        public int PackageID { get; set; }
        public int PlaceID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Master Master { get; set; }
        public virtual Package Package { get; set; }
    }
}