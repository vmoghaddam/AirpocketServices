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
    
    public partial class FDP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FDP()
        {
            this.FDP1 = new HashSet<FDP>();
            this.IdeaSessionItems = new HashSet<IdeaSessionItem>();
            this.FDPItems = new HashSet<FDPItem>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CrewId { get; set; }
        public Nullable<System.DateTime> ReportingTime { get; set; }
        public Nullable<System.DateTime> DelayedReportingTime { get; set; }
        public Nullable<System.DateTime> RevisedDelayedReportingTime { get; set; }
        public Nullable<System.DateTime> FirstNotification { get; set; }
        public Nullable<System.DateTime> NextNotification { get; set; }
        public Nullable<int> DelayAmount { get; set; }
        public Nullable<int> BoxId { get; set; }
        public Nullable<int> JobGroupId { get; set; }
        public bool IsTemplate { get; set; }
        public int DutyType { get; set; }
        public Nullable<System.DateTime> DateContact { get; set; }
        public Nullable<int> FDPId { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> TemplateId { get; set; }
        public Nullable<System.DateTime> FDPReportingTime { get; set; }
        public Nullable<System.Guid> GUID { get; set; }
        public Nullable<int> FirstFlightId { get; set; }
        public Nullable<int> LastFlightId { get; set; }
        public Nullable<int> UPD { get; set; }
        public Nullable<bool> IsMain { get; set; }
        public string Key { get; set; }
        public Nullable<bool> CP { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Remark { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> InitStart { get; set; }
        public Nullable<System.DateTime> InitEnd { get; set; }
        public Nullable<System.DateTime> InitRestTo { get; set; }
        public string InitFlts { get; set; }
        public string InitRoute { get; set; }
        public string InitFromIATA { get; set; }
        public string InitToIATA { get; set; }
        public string InitNo { get; set; }
        public string InitKey { get; set; }
        public Nullable<int> InitHomeBase { get; set; }
        public string InitRank { get; set; }
        public Nullable<int> InitIndex { get; set; }
        public string InitGroup { get; set; }
        public string InitScheduleName { get; set; }
        public string InitFlights { get; set; }
        public string Remark2 { get; set; }
        public string CanceledNo { get; set; }
        public string CanceledRoute { get; set; }
        public Nullable<int> Extension { get; set; }
        public Nullable<double> Split { get; set; }
        public Nullable<System.DateTime> DateConfirmed { get; set; }
        public string ConfirmedBy { get; set; }
        public string UserName { get; set; }
        public Nullable<decimal> MaxFDP { get; set; }
        public Nullable<int> BL { get; set; }
        public Nullable<int> FX { get; set; }
        public Nullable<System.DateTime> ActualStart { get; set; }
        public Nullable<System.DateTime> ActualEnd { get; set; }
        public Nullable<System.DateTime> ActualRestTo { get; set; }
        public Nullable<bool> IsOver { get; set; }
        public Nullable<System.DateTime> STD { get; set; }
        public Nullable<System.DateTime> STA { get; set; }
        public Nullable<bool> OutOfHomeBase { get; set; }
        public string InitPosition { get; set; }
        public Nullable<System.DateTime> PLNEnd { get; set; }
        public Nullable<System.DateTime> PLNRest { get; set; }
        public string PosFrom { get; set; }
        public string PosTo { get; set; }
        public Nullable<System.DateTime> PosDep { get; set; }
        public Nullable<System.DateTime> PosArr { get; set; }
        public string PosAirline { get; set; }
        public Nullable<int> PosFDPId { get; set; }
        public string PosRemark { get; set; }
        public string PosTicketUrl { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FDP> FDP1 { get; set; }
        public virtual FDP FDP2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaSessionItem> IdeaSessionItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FDPItem> FDPItems { get; set; }
    }
}
