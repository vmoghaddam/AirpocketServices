//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiFDM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FDMRegEventMonthly1
    {
        public Nullable<int> Year { get; set; }
        public string YearMonthName { get; set; }
        public Nullable<int> Month { get; set; }
        public string MonthName { get; set; }
        public string EventName { get; set; }
        public string register { get; set; }
        public int registerId { get; set; }
        public Nullable<int> FlightCount { get; set; }
        public Nullable<int> EventCount { get; set; }
        public Nullable<int> HighCount { get; set; }
        public Nullable<int> MediumCount { get; set; }
        public Nullable<int> LowCount { get; set; }
        public Nullable<int> Score { get; set; }
    }
}