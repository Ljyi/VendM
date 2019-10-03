namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAPP : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MachineStockDetails", "ProductId", "dbo.Product");
            DropIndex("dbo.MachineStockDetails", new[] { "ProductId" });
            CreateTable(
                "dbo.APP",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(nullable: false, maxLength: 10),
                        Url = c.String(nullable: false, maxLength: 100),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.ProductCategory", "SortNumber", c => c.Int());
            AlterColumn("dbo.ProductCategory", "Status", c => c.Int());
            AlterColumn("dbo.ProductCategory", "ProductCategoryVId", c => c.Int());
            AlterColumn("dbo.MachineStockDetails", "ProductId", c => c.Int());
            CreateIndex("dbo.MachineStockDetails", "ProductId");
            AddForeignKey("dbo.MachineStockDetails", "ProductId", "dbo.Product", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MachineStockDetails", "ProductId", "dbo.Product");
            DropIndex("dbo.MachineStockDetails", new[] { "ProductId" });
            AlterColumn("dbo.MachineStockDetails", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductCategory", "ProductCategoryVId", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductCategory", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductCategory", "SortNumber", c => c.Int(nullable: false));
            DropTable("dbo.APP");
            CreateIndex("dbo.MachineStockDetails", "ProductId");
            AddForeignKey("dbo.MachineStockDetails", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
    }
}
