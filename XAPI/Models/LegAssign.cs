//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LegAssign
    {
        public string Route { get; set; }
        public string Crew { get; set; }
        public string Pos { get; set; }
        public string Rank { get; set; }
        public string TurnType { get; set; }
        public string ScheduleGroup { get; set; }
        public string Scheduler { get; set; }
        public System.DateTime DateUTC { get; set; }
        public string FltNo { get; set; }
        public string DepStn { get; set; }
        public string ArrStn { get; set; }
        public Nullable<System.DateTime> DepTime { get; set; }
        public Nullable<System.DateTime> ArrTime { get; set; }
        public string ACType { get; set; }
        public string ACReg { get; set; }
        public string Flt { get; set; }
        public string RouteType { get; set; }
        public string JobType { get; set; }
        public Nullable<System.DateTime> DepTimeLCL { get; set; }
        public Nullable<System.DateTime> ArrTimeLCL { get; set; }
        public string Change { get; set; }
        public Nullable<System.DateTime> StandardTime { get; set; }
        public string Status { get; set; }
        public string Expr1 { get; set; }
        public Nullable<System.DateTime> OffBlock { get; set; }
        public Nullable<System.DateTime> OnBlock { get; set; }
    }
}
