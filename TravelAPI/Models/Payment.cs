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
    
    public partial class Payment
    {
        public int id { get; set; }
        public Nullable<int> BookingID { get; set; }
        public double AgreedAmount { get; set; }
        public double TotalAmount { get; set; }
        public Nullable<double> AdvanceAmount { get; set; }
        public Nullable<double> BalanceAmount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    
        public virtual Booking Booking { get; set; }
    }
}
