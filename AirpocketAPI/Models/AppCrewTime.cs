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
    
    public partial class AppCrewTime
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public string YearMonthName { get; set; }
        public int CrewId { get; set; }
        public string Name { get; set; }
        public string ScheduleName { get; set; }
        public int Flights { get; set; }
        public int BlockTime { get; set; }
        public int FlightTime { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
    }
}
