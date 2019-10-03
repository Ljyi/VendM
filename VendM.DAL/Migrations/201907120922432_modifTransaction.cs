namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "OrderNo", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Transaction", "TransactionInfo", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "TransactionInfo");
            DropColumn("dbo.Transaction", "OrderNo");
        }
    }
}
