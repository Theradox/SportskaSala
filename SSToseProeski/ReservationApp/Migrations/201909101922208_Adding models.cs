namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Organizator = c.String(nullable: false),
                        CurrentlyReserved = c.Int(nullable: false),
                        MaxCapacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Event_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservations", "Event_Id", "dbo.Events");
            DropIndex("dbo.Reservations", new[] { "User_Id" });
            DropIndex("dbo.Reservations", new[] { "Event_Id" });
            DropTable("dbo.Reservations");
            DropTable("dbo.Events");
        }
    }
}
