namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMachineDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MachineDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PassageNumber = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        MachineId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Machine", t => t.MachineId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.MachineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MachineDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.MachineDetail", "MachineId", "dbo.Machine");
            DropIndex("dbo.MachineDetail", new[] { "MachineId" });
            DropIndex("dbo.MachineDetail", new[] { "ProductId" });
            DropTable("dbo.MachineDetail");
        }
    }
}
