namespace ReservationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addadditionaldataannotationsformodelclasses : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Name", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Events", "Organizator", c => c.String(nullable: false, maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Organizator", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "Name", c => c.String(nullable: false));
        }
    }
}
