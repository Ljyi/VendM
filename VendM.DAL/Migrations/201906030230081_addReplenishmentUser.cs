namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReplenishmentUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Machine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplenishmentUserNo = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Static = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Machine");
        }
    }
}
