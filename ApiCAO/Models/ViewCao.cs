//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiCAO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewCao
    {
        public int ID { get; set; }
        public string Register { get; set; }
        public string AircraftType { get; set; }
        public string ToAirportIATA { get; set; }
        public string FromAirportIATA { get; set; }
        public string FlightNumber { get; set; }
        public string FlightStatus { get; set; }
        public Nullable<int> PaxAdult { get; set; }
        public Nullable<int> PaxInfant { get; set; }
        public Nullable<int> PaxChild { get; set; }
        public Nullable<int> TotalSeat { get; set; }
        public Nullable<int> BaggageCount { get; set; }
        public int BaggageWeight { get; set; }
        public Nullable<int> CargoCount { get; set; }
        public int CargoWeight { get; set; }
        public Nullable<int> OFPTAXIFUEL { get; set; }
        public Nullable<decimal> FuelUsed { get; set; }
        public Nullable<int> OFPTRIPFUEL { get; set; }
        public Nullable<decimal> FuelTotal { get; set; }
        public Nullable<decimal> FuelUplift { get; set; }
        public Nullable<System.DateTime> STD { get; set; }
        public Nullable<System.DateTime> STA { get; set; }
        public Nullable<int> FlightStatusID { get; set; }
        public Nullable<System.DateTime> BlockOffStation { get; set; }
        public Nullable<System.DateTime> BlockOnStation { get; set; }
        public Nullable<System.DateTime> LandingStation { get; set; }
        public Nullable<System.DateTime> TakeoffStation { get; set; }
    }
}