namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Point = c.Int(),
                        SaleType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.Order", "StoreNo", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.OrderDetails", "SaleType", c => c.Int(nullable: false));
            DropColumn("dbo.OrderDetails", "PriceType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "PriceType", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProductPrice", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductPrice", new[] { "ProductId" });
            DropColumn("dbo.OrderDetails", "SaleType");
            DropColumn("dbo.Order", "StoreNo");
            DropTable("dbo.ProductPrice");
        }
    }
}
