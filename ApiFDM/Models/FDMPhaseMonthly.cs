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
    
    public partial class FDMPhaseMonthly
    {
        public string Phase { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public int CrewId { get; set; }
        public string JobGroup { get; set; }
        public Nullable<int> EventCount { get; set; }
        public Nullable<int> Score { get; set; }
    }
}