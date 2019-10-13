namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyApplicationUsermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
        }
    }
}
