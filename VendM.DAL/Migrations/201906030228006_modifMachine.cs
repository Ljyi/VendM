namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifMachine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Machine", "Password", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Machine", "Password");
        }
    }
}
