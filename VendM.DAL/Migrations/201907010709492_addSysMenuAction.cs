namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSysMenuAction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRights", "SysButtonId", "dbo.SysButton");
            DropForeignKey("dbo.UserRights", "SysMenuId", "dbo.SysMenus");
            DropIndex("dbo.UserRights", new[] { "SysMenuId" });
            DropIndex("dbo.UserRights", new[] { "SysButtonId" });
            CreateTable(
                "dbo.SysMenuAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControlName = c.String(nullable: false),
                        ActionName = c.String(nullable: false),
                        SortNumber = c.Int(),
                        Status = c.String(),
                        SysMenuId = c.Int(nullable: false),
                        SysButtonId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SysButton", t => t.SysButtonId, cascadeDelete: true)
                .ForeignKey("dbo.SysMenus", t => t.SysMenuId, cascadeDelete: true)
                .Index(t => t.SysMenuId)
                .Index(t => t.SysButtonId);
            
            AddColumn("dbo.UserRights", "SysMenuActionId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserRights", "UserId");
            CreateIndex("dbo.UserRights", "SysMenuActionId");
            AddForeignKey("dbo.UserRights", "SysMenuActionId", "dbo.SysMenuAction", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRights", "UserId", "dbo.User", "Id", cascadeDelete: true);
            DropColumn("dbo.UserRights", "SysMenuId");
            DropColumn("dbo.UserRights", "SysButtonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRights", "SysButtonId", c => c.Int(nullable: false));
            AddColumn("dbo.UserRights", "SysMenuId", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserRights", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRights", "SysMenuActionId", "dbo.SysMenuAction");
            DropForeignKey("dbo.SysMenuAction", "SysMenuId", "dbo.SysMenus");
            DropForeignKey("dbo.SysMenuAction", "SysButtonId", "dbo.SysButton");
            DropIndex("dbo.UserRights", new[] { "SysMenuActionId" });
            DropIndex("dbo.UserRights", new[] { "UserId" });
            DropIndex("dbo.SysMenuAction", new[] { "SysButtonId" });
            DropIndex("dbo.SysMenuAction", new[] { "SysMenuId" });
            DropColumn("dbo.UserRights", "SysMenuActionId");
            DropTable("dbo.SysMenuAction");
            CreateIndex("dbo.UserRights", "SysButtonId");
            CreateIndex("dbo.UserRights", "SysMenuId");
            AddForeignKey("dbo.UserRights", "SysMenuId", "dbo.SysMenus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRights", "SysButtonId", "dbo.SysButton", "Id", cascadeDelete: true);
        }
    }
}
