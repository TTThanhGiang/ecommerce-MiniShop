﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThanhGiang_WebCuoiKi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbMiniShopEntities2 : DbContext
    {
        public dbMiniShopEntities2()
            : base("name=dbMiniShopEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbBAIDANG> tbBAIDANGs { get; set; }
        public virtual DbSet<tbCHITIETHOADON> tbCHITIETHOADONs { get; set; }
        public virtual DbSet<tbCHUYENMUC> tbCHUYENMUCs { get; set; }
        public virtual DbSet<tbDANHMUC> tbDANHMUCs { get; set; }
        public virtual DbSet<tbGIOHANG> tbGIOHANGs { get; set; }
        public virtual DbSet<tbHOADON> tbHOADONs { get; set; }
        public virtual DbSet<tbKHACHHANG> tbKHACHHANGs { get; set; }
        public virtual DbSet<tbNGUOIDUNG> tbNGUOIDUNGs { get; set; }
        public virtual DbSet<tbNHANVIEN> tbNHANVIENs { get; set; }
        public virtual DbSet<tbPHONGBAN> tbPHONGBANs { get; set; }
        public virtual DbSet<tbSANPHAM> tbSANPHAMs { get; set; }
    }
}
