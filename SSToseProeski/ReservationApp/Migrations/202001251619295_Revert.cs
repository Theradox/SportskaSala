namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Revert : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "EventTime");
            DropColumn("dbo.Reservations", "Paid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "Paid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Events", "EventTime", c => c.DateTime(nullable: false));
        }
    }
}
