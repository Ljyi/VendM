namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addApiLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.APILog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        APIName = c.String(nullable: false, maxLength: 100),
                        LogContent = c.String(nullable: false),
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
            DropTable("dbo.APILog");
        }
    }
}
