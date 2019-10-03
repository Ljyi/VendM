namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modfyApiLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.APILog", "MachineNo", c => c.String());
            AddColumn("dbo.APILog", "Source", c => c.Int(nullable: false));
            AddColumn("dbo.APILog", "RequestData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.APILog", "RequestData");
            DropColumn("dbo.APILog", "Source");
            DropColumn("dbo.APILog", "MachineNo");
        }
    }
}
