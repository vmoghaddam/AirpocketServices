//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiQA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewDutyLog
    {
        public Nullable<int> FlightId { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string Username { get; set; }
        public Nullable<int> CrewId { get; set; }
        public string CrewName { get; set; }
        public Nullable<int> PositionId { get; set; }
        public string Position { get; set; }
        public Nullable<bool> IsPositioning { get; set; }
        public string Action { get; set; }
        public Nullable<int> FlightStatusId { get; set; }
        public string FlightIds { get; set; }
        public string Flights { get; set; }
        public string Route { get; set; }
        public Nullable<System.DateTime> InitStart { get; set; }
        public Nullable<System.DateTime> InitEnd { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<int> DutyTypeId { get; set; }
        public int Id { get; set; }
        public string DutyType { get; set; }
    }
}