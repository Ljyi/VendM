namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            AddColumn("dbo.Order", "CardNo", c => c.String());
            AlterColumn("dbo.Product", "ProductCategoryId", c => c.Int());
            CreateIndex("dbo.Product", "ProductCategoryId");
            AddForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            AlterColumn("dbo.Product", "ProductCategoryId", c => c.Int(nullable: false));
            DropColumn("dbo.Order", "CardNo");
            CreateIndex("dbo.Product", "ProductCategoryId");
            AddForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory", "Id", cascadeDelete: true);
        }
    }
}
