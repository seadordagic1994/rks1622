using CondorExtreme3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Web.Configuration;

namespace CondorExtreme3.DAL
{
    public class CondorDBContext : DbContext
    {
        
        public CondorDBContext() : base("Master") { }

        public DbSet<Models.City> Cities { get; set; }

        public DbSet<Models.Country> Countries { get; set; }

        public DbSet<Models.Address> Addresses { get; set; }

        public DbSet<Models.Cinema> Cinemas { get; set; }

        public DbSet<Models.Administrator> Administrators { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            
            
            
        }
    }
}