namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyProduct1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "StoreId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "StoreId");
        }
    }
}
