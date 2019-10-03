namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifSysMenuAction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(nullable: false));
            AlterColumn("dbo.Product", "Specification_CH", c => c.String(maxLength: 500));
            AlterColumn("dbo.Product", "Specification_EN", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Specification_EN", c => c.String());
            AlterColumn("dbo.Product", "Specification_CH", c => c.String());
            DropColumn("dbo.SysMenuAction", "AuthorizeCode");
        }
    }
}
