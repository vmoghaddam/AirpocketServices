//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirpocketTRN.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewCourseRemaining
    {
        public int Id { get; set; }
        public Nullable<int> PersonId { get; set; }
        public int CourseId { get; set; }
        public int CourseTypeId { get; set; }
        public Nullable<int> CertificateTypeId { get; set; }
        public string CourseType { get; set; }
        public string CertificateType { get; set; }
        public string Title { get; set; }
        public string No { get; set; }
        public string PID { get; set; }
        public string JobGroup { get; set; }
        public string JobGroupCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public Nullable<System.DateTime> DateIssue { get; set; }
        public Nullable<System.DateTime> DateExpire { get; set; }
        public Nullable<int> Remaining { get; set; }
    }
}
