﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class ppa_entities : DbContext
    {
        public ppa_entities()
            : base("name=ppa_entities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Ac_MSN> Ac_MSN { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FlightChangeHistory> FlightChangeHistories { get; set; }
        public virtual DbSet<FlightStatusLog> FlightStatusLogs { get; set; }
        public virtual DbSet<ViewLegTime> ViewLegTimes { get; set; }
        public virtual DbSet<FDP> FDPs { get; set; }
        public virtual DbSet<FlightDelay> FlightDelays { get; set; }
        public virtual DbSet<ViewFDPItem> ViewFDPItems { get; set; }
        public virtual DbSet<CrewPickupSM> CrewPickupSMS { get; set; }
        public virtual DbSet<OffItem> OffItems { get; set; }
        public virtual DbSet<FDPExtra> FDPExtras { get; set; }
        public virtual DbSet<HelperMaxFDP> HelperMaxFDPs { get; set; }
        public virtual DbSet<ViewRegisterGround> ViewRegisterGrounds { get; set; }
        public virtual DbSet<ViewFlightInformation> ViewFlightInformations { get; set; }
        public virtual DbSet<FDPItem> FDPItems { get; set; }
        public virtual DbSet<EFBDSPRelease> EFBDSPReleases { get; set; }
        public virtual DbSet<ViewEmployeeLight> ViewEmployeeLights { get; set; }
        public virtual DbSet<ViewEFBDSPReleas> ViewEFBDSPReleases { get; set; }
        public virtual DbSet<AppCrewFlight> AppCrewFlights { get; set; }
        public virtual DbSet<AppLeg> AppLegs { get; set; }
        public virtual DbSet<FlightInformation> FlightInformations { get; set; }
        public virtual DbSet<ViewFlightsGantt> ViewFlightsGantts { get; set; }
    }
}