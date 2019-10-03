namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifMessageQueue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageQueue", "QueueName", c => c.String(nullable: false));
            AddColumn("dbo.MessageQueue", "MachineNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessageQueue", "MachineNo");
            DropColumn("dbo.MessageQueue", "QueueName");
        }
    }
}
