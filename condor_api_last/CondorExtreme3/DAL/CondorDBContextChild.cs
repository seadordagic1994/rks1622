using CondorExtreme3.ModelsLocalDB;
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

    public class CondorDBContextChild : DbContext
    {
        public CondorDBContextChild() : base("cnCineStarMostar") { }
        public CondorDBContextChild(string s) : base(s) { }

        public DbSet<ModelsLocalDB.Projection> Projections { get; set; }
        public DbSet<ModelsLocalDB.Director> Directors { get; set; }
        public DbSet<ModelsLocalDB.Actor> Actors { get; set; }
        public DbSet<ModelsLocalDB.Genre> Genres { get; set; }
        public DbSet<ModelsLocalDB.Movie> Movies { get; set; }
        public DbSet<ModelsLocalDB.MovieRole> MovieRoles { get; set; }
        public DbSet<ModelsLocalDB.CinemaHall> CinemaHalls { get; set; }
        public DbSet<ModelsLocalDB.CinemaHallTechnologyType> CinemaHallTechnologyTypes { get; set; }
        public DbSet<ModelsLocalDB.TechnologyType> TechnologyTypes { get; set; }
        public DbSet<ModelsLocalDB.MovieDirection> MovieDirections { get; set; }
        public DbSet<ModelsLocalDB.DefinedDateTime> DefinedDateTimes { get; set; }

        public DbSet<ModelsLocalDB.SeatRow> SeatRows { get; set; }
        public DbSet<ModelsLocalDB.SeatColumn> SeatColumns { get; set; }
        public DbSet<ModelsLocalDB.SeatType> SeatTypes { get; set; }
        public DbSet<ModelsLocalDB.Seat> Seats { get; set; }
        public DbSet<ModelsLocalDB.ProjectionSeat> ProjectionSeats { get; set; }

        public DbSet<ModelsLocalDB.Role> Roles { get; set; }
        public DbSet<ModelsLocalDB.EmployeeRole> EmployeesRoles { get; set; }
        public DbSet<ModelsLocalDB.Employee> Employees { get; set; }
       



        public DbSet<ModelsLocalDB.Visitor> Visitors { get; set; }
        public DbSet<ModelsLocalDB.RegisteredVisitor> RegisteredVisitors { get; set; }



     
        

        public DbSet<ModelsLocalDB.Reservation> Reservations { get; set; }
       
        public DbSet<ModelsLocalDB.PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ModelsLocalDB.Ticket> Tickets { get; set; }


        public DbSet<ModelsLocalDB.DiscountType> DiscountTypes { get; set; }
        public DbSet<ModelsLocalDB.Discount> Discounts { get; set; }


        public DbSet<ModelsLocalDB.Cinema> Cinemas { get; set; }
        public DbSet<ModelsLocalDB.Address> Addresses { get; set; }
        public DbSet<ModelsLocalDB.City> Cities { get; set; }
        public DbSet<ModelsLocalDB.Country> Country { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();




        }
    }
}