//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiLogUTC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Ac_MSN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ac_MSN()
        {
            this.FlightInformations = new HashSet<FlightInformation>();
        }

        public int ID { get; set; }
        public Nullable<int> AC_ModelID { get; set; }
        public Nullable<System.Guid> pkAircraftMSN { get; set; }
        public Nullable<int> fkFlight_Range { get; set; }
        public int AirlineOperatorsID { get; set; }
        public Nullable<int> fkAc_MSN_Status { get; set; }
        public Nullable<int> MSN { get; set; }
        public string Register { get; set; }
        public Nullable<int> TFH_Hours { get; set; }
        public Nullable<byte> TFH_Minutes { get; set; }
        public Nullable<int> TFC { get; set; }
        public Nullable<System.DateTime> ManDate { get; set; }
        public Nullable<System.DateTime> Last_WB { get; set; }
        public Nullable<bool> ETOPS { get; set; }
        public Nullable<bool> AC_Flag { get; set; }
        public Nullable<int> Cabin_Seat_Ver_F { get; set; }
        public Nullable<int> Cabin_Seat_Ver_B { get; set; }
        public Nullable<int> Cabin_Seat_Ver_C { get; set; }
        public Nullable<int> Cabin_Seat_Ver_R { get; set; }
        public Nullable<int> Lav_QTY { get; set; }
        public Nullable<int> Galley_QTY { get; set; }
        public Nullable<int> Cabin_CrewVer { get; set; }
        public Nullable<int> Cockpit_Seat_Ver_Pilot { get; set; }
        public Nullable<int> Cockpit_Seat_Ver_FlightEngineer { get; set; }
        public Nullable<int> Cockpit_Seat_Ver_Observer { get; set; }
        public string Previous_Register { get; set; }
        public Nullable<int> Fuel_LH_Outer { get; set; }
        public Nullable<int> Fuel_LH_Inner { get; set; }
        public Nullable<int> Fuel_Center { get; set; }
        public Nullable<int> Fuel_RH_Inner { get; set; }
        public Nullable<int> Fuel_RH_Outer { get; set; }
        public Nullable<int> Fuel_ACT1 { get; set; }
        public Nullable<int> Fuel_ACT2 { get; set; }
        public Nullable<int> Fuel_Trim { get; set; }
        public Nullable<int> Fuel_Total { get; set; }
        public Nullable<bool> FuelUnit { get; set; }
        public int CustomerId { get; set; }
        public Nullable<bool> IsVirtual { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<int> TotalSeat { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public Nullable<int> MAXZFW { get; set; }
        public Nullable<int> MAXLNW { get; set; }
        public Nullable<int> OASecLimit { get; set; }
        public Nullable<int> OBSecLimit { get; set; }
        public Nullable<int> OCSecLimit { get; set; }
        public Nullable<int> ODSecLimit { get; set; }
        public Nullable<int> CPT1Limit { get; set; }
        public Nullable<int> CPT2Limit { get; set; }
        public Nullable<int> CPT3Limit { get; set; }
        public Nullable<int> CPT4Limit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlightInformation> FlightInformations { get; set; }
    }
}