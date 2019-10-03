namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReplenishment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Replenishment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        MachineId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Machine", t => t.MachineId, cascadeDelete: true)
                .Index(t => t.MachineId);
            
            CreateTable(
                "dbo.ReplenishmentDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ProductNo = c.String(nullable: false),
                        ProductName = c.String(nullable: false),
                        PassageNumber = c.Int(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        InventoryQuantity = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ReplenishmentId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Replenishment", t => t.ReplenishmentId, cascadeDelete: true)
                .Index(t => t.ReplenishmentId);
            
            AddColumn("dbo.MachineStock", "RealStockQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReplenishmentDetail", "ReplenishmentId", "dbo.Replenishment");
            DropForeignKey("dbo.Replenishment", "MachineId", "dbo.Machine");
            DropIndex("dbo.ReplenishmentDetail", new[] { "ReplenishmentId" });
            DropIndex("dbo.Replenishment", new[] { "MachineId" });
            DropColumn("dbo.MachineStock", "RealStockQuantity");
            DropTable("dbo.ReplenishmentDetail");
            DropTable("dbo.Replenishment");
        }
    }
}
