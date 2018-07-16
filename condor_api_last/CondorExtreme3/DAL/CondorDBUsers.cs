using CondorExtreme3.ModelsUser;
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
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;
using System.Web.Mvc;

namespace CondorExtreme3.DAL
{
    public class CondorDBUsers : DbContext
    {
        public CondorDBUsers() : base("cnUsers") { }
        public DbSet<ModelsUser.VirtualPointsPack> VirtualPointsPacks { get; set; }
        public DbSet<ModelsUser.Country> Country { get; set; }
        public DbSet<ModelsUser.City> Cities { get; set; }
        public DbSet<ModelsUser.RegisteredVisitor> RegisteredVisitors { get; set; }
        public DbSet<ModelsUser.SalesOrderVirtualPoints> SalesOrderVirtualPoints { get; set; }

        public DbSet<ModelsUser.Reservation> Reservations { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

    }
}