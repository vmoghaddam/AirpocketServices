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
    
    public partial class ViewQAMaintenance
    {
        public Nullable<int> Id { get; set; }
        public int FlightId { get; set; }
        public Nullable<System.DateTime> DateReport { get; set; }
        public Nullable<System.DateTime> DateSign { get; set; }
        public string AircraftType { get; set; }
        public string Register { get; set; }
        public Nullable<int> ComponentSpecificationId { get; set; }
        public string ComponentSpecification { get; set; }
        public string FlightRoute { get; set; }
        public string FlightNumber { get; set; }
        public string ATLNo { get; set; }
        public string TaskNo { get; set; }
        public string Reference { get; set; }
        public Nullable<int> StationId { get; set; }
        public string Station { get; set; }
        public string EventDescription { get; set; }
        public string ActionTakenDescription { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string Name { get; set; }
        public string CAALicenceNo { get; set; }
        public string AuthorizationNo { get; set; }
        public Nullable<int> SerialNumber { get; set; }
        public Nullable<int> PartNumber { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> STD { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> dateStatus { get; set; }
        public Nullable<int> StatusEmployeeId { get; set; }
        public string StatusEmployeeName { get; set; }
        public string Result { get; set; }
    }
}