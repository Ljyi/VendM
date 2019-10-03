namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMessageQueue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageQueue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        APIName = c.String(nullable: false),
                        MQType = c.String(nullable: false),
                        Message = c.String(),
                        Status = c.Int(nullable: false),
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
            DropTable("dbo.MessageQueue");
        }
    }
}
