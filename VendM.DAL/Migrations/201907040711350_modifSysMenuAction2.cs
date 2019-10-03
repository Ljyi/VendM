namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifSysMenuAction2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MachineDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.SysMenuAction", "SysButtonId", "dbo.SysButton");
            DropIndex("dbo.MachineDetail", new[] { "ProductId" });
            DropIndex("dbo.SysMenuAction", new[] { "SysButtonId" });
            AlterColumn("dbo.MachineDetail", "ProductId", c => c.Int());
            AlterColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(maxLength: 10));
            AlterColumn("dbo.SysMenuAction", "SysButtonId", c => c.Int());
            CreateIndex("dbo.MachineDetail", "ProductId");
            CreateIndex("dbo.SysMenuAction", "SysButtonId");
            AddForeignKey("dbo.MachineDetail", "ProductId", "dbo.Product", "Id");
            AddForeignKey("dbo.SysMenuAction", "SysButtonId", "dbo.SysButton", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysMenuAction", "SysButtonId", "dbo.SysButton");
            DropForeignKey("dbo.MachineDetail", "ProductId", "dbo.Product");
            DropIndex("dbo.SysMenuAction", new[] { "SysButtonId" });
            DropIndex("dbo.MachineDetail", new[] { "ProductId" });
            AlterColumn("dbo.SysMenuAction", "SysButtonId", c => c.Int(nullable: false));
            AlterColumn("dbo.SysMenuAction", "AuthorizeCode", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.MachineDetail", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.SysMenuAction", "SysButtonId");
            CreateIndex("dbo.MachineDetail", "ProductId");
            AddForeignKey("dbo.SysMenuAction", "SysButtonId", "dbo.SysButton", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MachineDetail", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
    }
}
