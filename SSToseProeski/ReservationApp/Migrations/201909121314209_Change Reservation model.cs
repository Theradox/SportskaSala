namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeReservationmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "NoOfTickets", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "NoOfTickets");
        }
    }
}
