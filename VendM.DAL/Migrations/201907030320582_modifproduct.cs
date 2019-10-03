namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifproduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Specification_CH", c => c.String());
            AddColumn("dbo.Product", "Specification_EN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Specification_EN");
            DropColumn("dbo.Product", "Specification_CH");
        }
    }
}
