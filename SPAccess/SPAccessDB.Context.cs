﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    internal partial class SPAccessDB : DbContext
    {
        public SPAccessDB()
            : base("name=SPAccessDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GroupIdentity> GroupIdentity { get; set; }
        public virtual DbSet<PermissionIdentity> PermissionIdentity { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UserIdentity> UserIdentity { get; set; }
        public virtual DbSet<GroupInformation> GroupInformation { get; set; }
        public virtual DbSet<PermissionInformation> PermissionInformation { get; set; }
        public virtual DbSet<UserInformation> UserInformation { get; set; }
    }
}
