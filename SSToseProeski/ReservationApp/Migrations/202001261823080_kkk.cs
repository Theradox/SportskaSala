namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kkk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventTime", c => c.String(nullable: false));
            AddColumn("dbo.Reservations", "Paid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Paid");
            DropColumn("dbo.Events", "EventTime");
        }
    }
}
