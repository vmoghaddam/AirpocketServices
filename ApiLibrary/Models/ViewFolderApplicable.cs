//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ViewFolderApplicable
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string FullCode { get; set; }
        public int EmployeeId { get; set; }
        public Nullable<int> Items { get; set; }
        public Nullable<int> Files { get; set; }
        public Nullable<int> NotVisited { get; set; }
        public Nullable<int> NotDownloaded { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> EmployeeCustomerId { get; set; }
    }
}
