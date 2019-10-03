namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "IsAdmin", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SysMenus", "Icon", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SysMenus", "Icon", c => c.String());
            DropColumn("dbo.User", "IsAdmin");
        }
    }
}
