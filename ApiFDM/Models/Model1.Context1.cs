﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiFDM.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntities : DbContext
    {
        public dbEntities()
            : base("name=dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CrewSecretCode> CrewSecretCodes { get; set; }
        public virtual DbSet<FlightInformation> FlightInformations { get; set; }
        public virtual DbSet<Ac_MSN> Ac_MSN { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<ViewLegTime> ViewLegTimes { get; set; }
        public virtual DbSet<AppLeg> AppLegs { get; set; }
        public virtual DbSet<FDM> FDMs { get; set; }
        public virtual DbSet<ViewFDM> ViewFDMs { get; set; }
    }
}