namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenderpropertytoApplicationUsermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
        }
    }
}
