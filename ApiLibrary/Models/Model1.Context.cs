﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiLibrary.Models
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
    
        public virtual DbSet<ViewBookFile> ViewBookFiles { get; set; }
        public virtual DbSet<HelperBookApplicableEmployee> HelperBookApplicableEmployees { get; set; }
        public virtual DbSet<ViewFolderApplicable> ViewFolderApplicables { get; set; }
        public virtual DbSet<ViewBookApplicableEmployee> ViewBookApplicableEmployees { get; set; }
        public virtual DbSet<BookChapter> BookChapters { get; set; }
        public virtual DbSet<BookFile> BookFiles { get; set; }
        public virtual DbSet<ViewBookChapter> ViewBookChapters { get; set; }
        public virtual DbSet<ViewBookFileVisited> ViewBookFileVisiteds { get; set; }
    }
}