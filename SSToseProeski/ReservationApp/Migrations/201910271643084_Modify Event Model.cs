namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyEventModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Image", c => c.String(nullable: false));
            AddColumn("dbo.Events", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Description");
            DropColumn("dbo.Events", "Price");
            DropColumn("dbo.Events", "Image");
        }
    }
}
