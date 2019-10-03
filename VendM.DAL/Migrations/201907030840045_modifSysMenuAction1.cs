namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifSysMenuAction1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SysMenuAction", "ControlName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.SysMenuAction", "ActionName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(nullable: false));
            AlterColumn("dbo.SysMenuAction", "ActionName", c => c.String(nullable: false));
            AlterColumn("dbo.SysMenuAction", "ControlName", c => c.String(nullable: false));
        }
    }
}
