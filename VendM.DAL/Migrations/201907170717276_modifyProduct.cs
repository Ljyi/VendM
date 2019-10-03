namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductVId", c => c.String());
            AddColumn("dbo.ProductCategory", "ProductCategoryVId", c => c.Int(nullable: false));
            AddColumn("dbo.ProductPrice", "ProductWayVId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductPrice", "ProductWayVId");
            DropColumn("dbo.ProductCategory", "ProductCategoryVId");
            DropColumn("dbo.Product", "ProductVId");
        }
    }
}
