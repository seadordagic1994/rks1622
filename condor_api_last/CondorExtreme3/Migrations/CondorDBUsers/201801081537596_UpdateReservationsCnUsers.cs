namespace CondorExtreme3.Migrations.CondorDBUsers
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReservationsCnUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Guid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Guid");
        }
    }
}
