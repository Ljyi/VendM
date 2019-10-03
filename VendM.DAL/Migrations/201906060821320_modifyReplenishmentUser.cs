namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyReplenishmentUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReplenishmentUser", "Status", c => c.Int(nullable: false));
            CreateIndex("dbo.InventoryChangeLog", "ProductId");
            AddForeignKey("dbo.InventoryChangeLog", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
            DropColumn("dbo.ReplenishmentUser", "Static");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReplenishmentUser", "Static", c => c.Int(nullable: false));
            DropForeignKey("dbo.InventoryChangeLog", "ProductId", "dbo.Product");
            DropIndex("dbo.InventoryChangeLog", new[] { "ProductId" });
            DropColumn("dbo.ReplenishmentUser", "Status");
        }
    }
}
