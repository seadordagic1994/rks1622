namespace CondorExtreme3.Migrations.CondorDBUsers
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PostalCode = c.String(),
                        CountryID = c.Int(nullable: false),
                        Guid = c.String(),
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
                        Guid = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryID);
            
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
                "dbo.SalesOrderVirtualPoints",
                c => new
                    {
                        SalesOrderNumber = c.Int(nullable: false, identity: true),
                        VirtualPointsPackID = c.Int(nullable: false),
                        RegisteredVisitorID = c.Int(nullable: false),
                        Guid = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SalesOrderNumber)
                .ForeignKey("dbo.RegisteredVisitors", t => t.RegisteredVisitorID)
                .ForeignKey("dbo.VirtualPointsPacks", t => t.VirtualPointsPackID)
                .Index(t => t.VirtualPointsPackID)
                .Index(t => t.RegisteredVisitorID);
            
            CreateTable(
                "dbo.VirtualPointsPacks",
                c => new
                    {
                        VirtualPointsPackID = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Guid = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VirtualPointsPackID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesOrderVirtualPoints", "VirtualPointsPackID", "dbo.VirtualPointsPacks");
            DropForeignKey("dbo.SalesOrderVirtualPoints", "RegisteredVisitorID", "dbo.RegisteredVisitors");
            DropForeignKey("dbo.RegisteredVisitors", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryID", "dbo.Countries");
            DropIndex("dbo.SalesOrderVirtualPoints", new[] { "RegisteredVisitorID" });
            DropIndex("dbo.SalesOrderVirtualPoints", new[] { "VirtualPointsPackID" });
            DropIndex("dbo.RegisteredVisitors", new[] { "CityID" });
            DropIndex("dbo.Cities", new[] { "CountryID" });
            DropTable("dbo.VirtualPointsPacks");
            DropTable("dbo.SalesOrderVirtualPoints");
            DropTable("dbo.RegisteredVisitors");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
        }
    }
}
