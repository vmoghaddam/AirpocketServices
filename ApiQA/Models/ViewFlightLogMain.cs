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
    
    public partial class ViewFlightLogMain
    {
        public int Id { get; set; }
        public Nullable<int> FlightId { get; set; }
        public string Username { get; set; }
        public Nullable<System.DateTime> STD { get; set; }
        public Nullable<System.DateTime> STA { get; set; }
        public Nullable<int> FromId { get; set; }
        public Nullable<int> ToId { get; set; }
        public Nullable<int> RegisterId { get; set; }
        public string FlightNo { get; set; }
        public Nullable<System.DateTime> iSTD { get; set; }
        public Nullable<System.DateTime> iSTA { get; set; }
        public Nullable<int> iFromId { get; set; }
        public Nullable<int> iToId { get; set; }
        public Nullable<int> iRegisterId { get; set; }
        public string iFlightNo { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateDelete { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Register { get; set; }
        public string iRegister { get; set; }
        public string FromIATA { get; set; }
        public string iFromIATA { get; set; }
        public string ToIATA { get; set; }
        public string iToIATA { get; set; }
        public Nullable<System.DateTime> STDLocal { get; set; }
        public Nullable<System.DateTime> STALocal { get; set; }
        public Nullable<System.DateTime> iSTDLocal { get; set; }
        public Nullable<System.DateTime> iSTALocal { get; set; }
        public string FlightStatus { get; set; }
        public Nullable<int> FlightStatusID { get; set; }
    }
}