namespace CondorExtreme3.Migrations.CondorDBUsers
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservationsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationID = c.Int(nullable: false, identity: true),
                        ConnString = c.String(),
                        RegisteredVisitorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationID)
                .ForeignKey("dbo.RegisteredVisitors", t => t.RegisteredVisitorID)
                .Index(t => t.RegisteredVisitorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "RegisteredVisitorID", "dbo.RegisteredVisitors");
            DropIndex("dbo.Reservations", new[] { "RegisteredVisitorID" });
            DropTable("dbo.Reservations");
        }
    }
}
