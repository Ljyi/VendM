namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifSysMenuAction11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(maxLength: 10));
        }
    }
}
