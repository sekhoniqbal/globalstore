﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class globalSpaceEntities : DbContext
    {
        public globalSpaceEntities()
            : base("name=globalSpaceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Transection> Transections { get; set; }
        public virtual DbSet<TransectionItem> TransectionItems { get; set; }
        public virtual DbSet<TransectionItemStatu> TransectionItemStatus { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
