namespace CondorExtreme3.Migrations.CondorDBContextChild
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actors",
                c => new
                    {
                        ActorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActorID);
            
            CreateTable(
                "dbo.MovieRoles",
                c => new
                    {
                        MovieID = c.Int(nullable: false),
                        ActorID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieID, t.ActorID })
                .ForeignKey("dbo.Actors", t => t.ActorID)
                .ForeignKey("dbo.Movies", t => t.MovieID)
                .Index(t => t.MovieID)
                .Index(t => t.ActorID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        MovieName = c.String(),
                        OriginalName = c.String(),
                        GenreID = c.Int(nullable: false),
                        DurationInMinutes = c.Int(nullable: false),
                        AgeRestriction = c.String(),
                        ReleaseYear = c.String(),
                        Synopsis = c.String(),
                        Picture = c.String(),
                        Trailler = c.String(),
                        IsCurrent = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MovieID)
                .ForeignKey("dbo.Genres", t => t.GenreID)
                .Index(t => t.GenreID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        GenreName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GenreID);
            
            CreateTable(
                "dbo.MovieDirection",
                c => new
                    {
                        MovieID = c.Int(nullable: false),
                        DirectorID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieID, t.DirectorID })
                .ForeignKey("dbo.Directors", t => t.DirectorID)
                .ForeignKey("dbo.Movies", t => t.MovieID)
                .Index(t => t.MovieID)
                .Index(t => t.DirectorID);
            
            CreateTable(
                "dbo.Directors",
                c => new
                    {
                        DirectorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DirectorID);
            
            CreateTable(
                "dbo.Projections",
                c => new
                    {
                        ProjectionID = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        CinemaHallID = c.Int(nullable: false),
                        DateTimeStart = c.DateTime(nullable: false),
                        DateTimeEnd = c.DateTime(nullable: false),
                        TechnologyTypeID = c.Int(nullable: false),
                        TicketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectionID)
                .ForeignKey("dbo.CinemaHalls", t => t.CinemaHallID)
                .ForeignKey("dbo.DefinedDateTimes", t => t.DateTimeStart)
                .ForeignKey("dbo.Movies", t => t.MovieID)
                .ForeignKey("dbo.TechnologyTypes", t => t.TechnologyTypeID)
                .Index(t => t.MovieID)
                .Index(t => t.CinemaHallID)
                .Index(t => t.DateTimeStart)
                .Index(t => t.TechnologyTypeID);
            
            CreateTable(
                "dbo.CinemaHalls",
                c => new
                    {
                        CinemaHallID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CinemaID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CinemaHallID)
                .ForeignKey("dbo.Cinemas", t => t.CinemaID)
                .Index(t => t.CinemaID);
            
            CreateTable(
                "dbo.Cinemas",
                c => new
                    {
                        CinemaID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AddressID = c.Int(nullable: false),
                        ConnectionString = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CinemaID)
                .ForeignKey("dbo.Addresses", t => t.AddressID)
                .Index(t => t.AddressID);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(),
                        CityID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PostalCode = c.String(),
                        CountryID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityID)
                .ForeignKey("dbo.Countries", t => t.CountryID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        SeatID = c.Int(nullable: false, identity: true),
                        CinemaHallID = c.Int(nullable: false),
                        SeatRowID = c.String(maxLength: 128),
                        SeatColumnID = c.Int(nullable: false),
                        SeatTypeID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SeatID)
                .ForeignKey("dbo.CinemaHalls", t => t.CinemaHallID)
                .ForeignKey("dbo.SeatColumns", t => t.SeatColumnID)
                .ForeignKey("dbo.SeatRows", t => t.SeatRowID)
                .ForeignKey("dbo.SeatTypes", t => t.SeatTypeID)
                .Index(t => t.CinemaHallID)
                .Index(t => t.SeatRowID)
                .Index(t => t.SeatColumnID)
                .Index(t => t.SeatTypeID);
            
            CreateTable(
                "dbo.ProjectionSeats",
                c => new
                    {
                        ProjectionID = c.Int(nullable: false),
                        SeatID = c.Int(nullable: false),
                        IsReserved = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectionID, t.SeatID })
                .ForeignKey("dbo.Projections", t => t.ProjectionID)
                .ForeignKey("dbo.Seats", t => t.SeatID)
                .Index(t => t.ProjectionID)
                .Index(t => t.SeatID);
            
            CreateTable(
                "dbo.SeatColumns",
                c => new
                    {
                        SeatColumnID = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SeatColumnID);
            
            CreateTable(
                "dbo.SeatRows",
                c => new
                    {
                        SeatRowID = c.String(nullable: false, maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SeatRowID);
            
            CreateTable(
                "dbo.SeatTypes",
                c => new
                    {
                        SeatTypeID = c.Int(nullable: false, identity: true),
                        SeatTypeName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SeatTypeID);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        SeatID = c.Int(nullable: false),
                        ReservationID = c.Int(nullable: false),
                        TicketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalTicketPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSold = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TicketID)
                .ForeignKey("dbo.Reservations", t => t.ReservationID)
                .ForeignKey("dbo.Seats", t => t.SeatID)
                .Index(t => t.SeatID)
                .Index(t => t.ReservationID);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountTypeID = c.Int(nullable: false),
                        TicketID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.DiscountTypeID, t.TicketID })
                .ForeignKey("dbo.DiscountTypes", t => t.DiscountTypeID)
                .ForeignKey("dbo.Tickets", t => t.TicketID)
                .Index(t => t.DiscountTypeID)
                .Index(t => t.TicketID);
            
            CreateTable(
                "dbo.DiscountTypes",
                c => new
                    {
                        DiscountTypeID = c.Int(nullable: false, identity: true),
                        DiscountName = c.String(),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DiscountTypeID);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationID = c.Int(nullable: false, identity: true),
                        VisitorID = c.Int(),
                        RegisteredVisitorID = c.Int(),
                        ProjectionID = c.Int(nullable: false),
                        PaymentMethodID = c.Int(nullable: false),
                        ReservationDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        ReservationCompleted = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationID)
                .ForeignKey("dbo.PaymentMethods", t => t.PaymentMethodID)
                .ForeignKey("dbo.Projections", t => t.ProjectionID)
                .ForeignKey("dbo.RegisteredVisitors", t => t.RegisteredVisitorID)
                .ForeignKey("dbo.Visitors", t => t.VisitorID)
                .Index(t => t.VisitorID)
                .Index(t => t.RegisteredVisitorID)
                .Index(t => t.ProjectionID)
                .Index(t => t.PaymentMethodID);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        PaymentMethodID = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentMethodID);
            
            CreateTable(
                "dbo.RegisteredVisitors",
                c => new
                    {
                        RegisteredVisitorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CityID = c.Int(nullable: false),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        VirtualPointsTotal = c.Int(nullable: false),
                        Guid = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RegisteredVisitorID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        VisitorID = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        ActivationCode = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VisitorID);
            
            CreateTable(
                "dbo.DefinedDateTimes",
                c => new
                    {
                        DateTimeStart = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DateTimeStart);
            
            CreateTable(
                "dbo.TechnologyTypes",
                c => new
                    {
                        TechnologyTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TechnologyTypeID);
            
            CreateTable(
                "dbo.CinemaHallsTechnologyTypes",
                c => new
                    {
                        TechnologyTypeID = c.Int(nullable: false),
                        CinemaHallID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.TechnologyTypeID, t.CinemaHallID })
                .ForeignKey("dbo.CinemaHalls", t => t.CinemaHallID)
                .ForeignKey("dbo.TechnologyTypes", t => t.TechnologyTypeID)
                .Index(t => t.TechnologyTypeID)
                .Index(t => t.CinemaHallID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CityID = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.Boolean(nullable: false),
                        HireDate = c.DateTime(nullable: false),
                        Salary = c.Double(nullable: false),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Picture = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.EmployeesRoles",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.EmployeeID })
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .Index(t => t.RoleID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeesRoles", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.EmployeesRoles", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "CityID", "dbo.Cities");
            DropForeignKey("dbo.MovieRoles", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.Projections", "TechnologyTypeID", "dbo.TechnologyTypes");
            DropForeignKey("dbo.CinemaHallsTechnologyTypes", "TechnologyTypeID", "dbo.TechnologyTypes");
            DropForeignKey("dbo.CinemaHallsTechnologyTypes", "CinemaHallID", "dbo.CinemaHalls");
            DropForeignKey("dbo.Projections", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.Projections", "DateTimeStart", "dbo.DefinedDateTimes");
            DropForeignKey("dbo.Projections", "CinemaHallID", "dbo.CinemaHalls");
            DropForeignKey("dbo.Tickets", "SeatID", "dbo.Seats");
            DropForeignKey("dbo.Reservations", "VisitorID", "dbo.Visitors");
            DropForeignKey("dbo.Tickets", "ReservationID", "dbo.Reservations");
            DropForeignKey("dbo.Reservations", "RegisteredVisitorID", "dbo.RegisteredVisitors");
            DropForeignKey("dbo.RegisteredVisitors", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Reservations", "ProjectionID", "dbo.Projections");
            DropForeignKey("dbo.Reservations", "PaymentMethodID", "dbo.PaymentMethods");
            DropForeignKey("dbo.Discounts", "TicketID", "dbo.Tickets");
            DropForeignKey("dbo.Discounts", "DiscountTypeID", "dbo.DiscountTypes");
            DropForeignKey("dbo.Seats", "SeatTypeID", "dbo.SeatTypes");
            DropForeignKey("dbo.Seats", "SeatRowID", "dbo.SeatRows");
            DropForeignKey("dbo.Seats", "SeatColumnID", "dbo.SeatColumns");
            DropForeignKey("dbo.ProjectionSeats", "SeatID", "dbo.Seats");
            DropForeignKey("dbo.ProjectionSeats", "ProjectionID", "dbo.Projections");
            DropForeignKey("dbo.Seats", "CinemaHallID", "dbo.CinemaHalls");
            DropForeignKey("dbo.CinemaHalls", "CinemaID", "dbo.Cinemas");
            DropForeignKey("dbo.Cinemas", "AddressID", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryID", "dbo.Countries");
            DropForeignKey("dbo.MovieDirection", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.MovieDirection", "DirectorID", "dbo.Directors");
            DropForeignKey("dbo.Movies", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.MovieRoles", "ActorID", "dbo.Actors");
            DropIndex("dbo.EmployeesRoles", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeesRoles", new[] { "RoleID" });
            DropIndex("dbo.Employees", new[] { "CityID" });
            DropIndex("dbo.CinemaHallsTechnologyTypes", new[] { "CinemaHallID" });
            DropIndex("dbo.CinemaHallsTechnologyTypes", new[] { "TechnologyTypeID" });
            DropIndex("dbo.RegisteredVisitors", new[] { "CityID" });
            DropIndex("dbo.Reservations", new[] { "PaymentMethodID" });
            DropIndex("dbo.Reservations", new[] { "ProjectionID" });
            DropIndex("dbo.Reservations", new[] { "RegisteredVisitorID" });
            DropIndex("dbo.Reservations", new[] { "VisitorID" });
            DropIndex("dbo.Discounts", new[] { "TicketID" });
            DropIndex("dbo.Discounts", new[] { "DiscountTypeID" });
            DropIndex("dbo.Tickets", new[] { "ReservationID" });
            DropIndex("dbo.Tickets", new[] { "SeatID" });
            DropIndex("dbo.ProjectionSeats", new[] { "SeatID" });
            DropIndex("dbo.ProjectionSeats", new[] { "ProjectionID" });
            DropIndex("dbo.Seats", new[] { "SeatTypeID" });
            DropIndex("dbo.Seats", new[] { "SeatColumnID" });
            DropIndex("dbo.Seats", new[] { "SeatRowID" });
            DropIndex("dbo.Seats", new[] { "CinemaHallID" });
            DropIndex("dbo.Cities", new[] { "CountryID" });
            DropIndex("dbo.Addresses", new[] { "CityID" });
            DropIndex("dbo.Cinemas", new[] { "AddressID" });
            DropIndex("dbo.CinemaHalls", new[] { "CinemaID" });
            DropIndex("dbo.Projections", new[] { "TechnologyTypeID" });
            DropIndex("dbo.Projections", new[] { "DateTimeStart" });
            DropIndex("dbo.Projections", new[] { "CinemaHallID" });
            DropIndex("dbo.Projections", new[] { "MovieID" });
            DropIndex("dbo.MovieDirection", new[] { "DirectorID" });
            DropIndex("dbo.MovieDirection", new[] { "MovieID" });
            DropIndex("dbo.Movies", new[] { "GenreID" });
            DropIndex("dbo.MovieRoles", new[] { "ActorID" });
            DropIndex("dbo.MovieRoles", new[] { "MovieID" });
            DropTable("dbo.Roles");
            DropTable("dbo.EmployeesRoles");
            DropTable("dbo.Employees");
            DropTable("dbo.CinemaHallsTechnologyTypes");
            DropTable("dbo.TechnologyTypes");
            DropTable("dbo.DefinedDateTimes");
            DropTable("dbo.Visitors");
            DropTable("dbo.RegisteredVisitors");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.Reservations");
            DropTable("dbo.DiscountTypes");
            DropTable("dbo.Discounts");
            DropTable("dbo.Tickets");
            DropTable("dbo.SeatTypes");
            DropTable("dbo.SeatRows");
            DropTable("dbo.SeatColumns");
            DropTable("dbo.ProjectionSeats");
            DropTable("dbo.Seats");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Addresses");
            DropTable("dbo.Cinemas");
            DropTable("dbo.CinemaHalls");
            DropTable("dbo.Projections");
            DropTable("dbo.Directors");
            DropTable("dbo.MovieDirection");
            DropTable("dbo.Genres");
            DropTable("dbo.Movies");
            DropTable("dbo.MovieRoles");
            DropTable("dbo.Actors");
        }
    }
}
