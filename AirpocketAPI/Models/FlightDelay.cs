//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirpocketAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FlightDelay
    {
        public int ID { get; set; }
        public int FlightId { get; set; }
        public int DelayCodeId { get; set; }
        public Nullable<int> HH { get; set; }
        public Nullable<int> MM { get; set; }
        public string Remark { get; set; }
        public Nullable<int> UserId { get; set; }
        public string ICategory { get; set; }
    
        public virtual DelayCode DelayCode { get; set; }
        public virtual FlightInformation FlightInformation { get; set; }
    }
}
