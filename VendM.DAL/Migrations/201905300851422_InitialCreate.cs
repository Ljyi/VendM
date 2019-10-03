namespace VendM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        AdvertisementNO = c.String(nullable: false, maxLength: 100),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Video",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoUrl = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        AdvertisementId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisement", t => t.AdvertisementId, cascadeDelete: true)
                .Index(t => t.AdvertisementId);
            
            CreateTable(
                "dbo.InventoryChangeLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ChangeType = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 200),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogType = c.String(nullable: false, maxLength: 100),
                        LogContent = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Machine",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        MachineNo = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        FaultType = c.Int(nullable: false),
                        FaultTime = c.DateTime(),
                        HandleTime = c.DateTime(),
                        Address = c.String(nullable: false, maxLength: 500),
                        Remark = c.String(maxLength: 200),
                        StoreId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 500),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MachineStock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThresholdPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalQuantity = c.Int(nullable: false),
                        Machine = c.String(maxLength: 50),
                        LastTime = c.DateTime(),
                        MachineId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Machine", t => t.MachineId, cascadeDelete: true)
                .Index(t => t.MachineId);
            
            CreateTable(
                "dbo.MachineStockDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalQuantity = c.Int(nullable: false),
                        InventoryQuantity = c.Int(nullable: false),
                        PassageNumber = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        MachineStockId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MachineStock", t => t.MachineStockId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.MachineStockId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(nullable: false, maxLength: 50),
                        ProductName_EN = c.String(nullable: false, maxLength: 100),
                        ProductName_CH = c.String(nullable: false, maxLength: 100),
                        ProductDetails_EN = c.String(nullable: false, maxLength: 500),
                        ProductDetails_CH = c.String(nullable: false, maxLength: 500),
                        ProductCategoryId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName_CN = c.String(nullable: false, maxLength: 200),
                        CategoryName_EN = c.String(nullable: false, maxLength: 200),
                        CategoryCode = c.String(nullable: false, maxLength: 20),
                        SortNumber = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductImage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 200),
                        Main = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(nullable: false, maxLength: 50),
                        TransactionId = c.Int(),
                        StoreName = c.String(nullable: false, maxLength: 50),
                        MachineNo = c.String(nullable: false, maxLength: 50),
                        PayMent = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Points = c.Decimal(precision: 18, scale: 2),
                        OrderStatus = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNo = c.String(nullable: false, maxLength: 50),
                        ProductCode = c.String(nullable: false, maxLength: 50),
                        ProductName = c.String(nullable: false, maxLength: 200),
                        PriceType = c.Int(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Points = c.Decimal(precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.PayMent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentName = c.String(nullable: false, maxLength: 50),
                        PaymentCode = c.String(),
                        Charge = c.String(maxLength: 20),
                        Fee = c.String(),
                        Status = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 100),
                        Status = c.String(nullable: false, maxLength: 100),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalQuantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.SysButton",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ButtonName = c.String(nullable: false, maxLength: 100),
                        ButtonIocn = c.String(maxLength: 100),
                        ButtonCode = c.String(nullable: false, maxLength: 100),
                        InputType = c.String(maxLength: 200),
                        Status = c.String(maxLength: 10),
                        ButtonStyle = c.String(maxLength: 100),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MenuCode = c.String(nullable: false, maxLength: 100),
                        Url = c.String(nullable: false, maxLength: 100),
                        MenuName = c.String(nullable: false, maxLength: 100),
                        MenuLevel = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        SortNumber = c.Int(nullable: false),
                        Icon = c.String(),
                        Status = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        LogingName = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Status = c.String(maxLength: 10),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SysMenuId = c.Int(nullable: false),
                        SysButtonId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateUser = c.String(),
                        CredateTime = c.DateTime(nullable: false),
                        UpdateUser = c.String(),
                        UpdateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserRights", "SysMenuId", "dbo.SysMenus");
            DropForeignKey("dbo.UserRights", "SysButtonId", "dbo.SysButton");
            DropForeignKey("dbo.Stock", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Order");
            DropForeignKey("dbo.MachineStockDetails", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductImage", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.MachineStockDetails", "MachineStockId", "dbo.MachineStock");
            DropForeignKey("dbo.MachineStock", "MachineId", "dbo.Machine");
            DropForeignKey("dbo.Machine", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Video", "AdvertisementId", "dbo.Advertisement");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRights", new[] { "SysButtonId" });
            DropIndex("dbo.UserRights", new[] { "SysMenuId" });
            DropIndex("dbo.Stock", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.ProductImage", new[] { "ProductId" });
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.MachineStockDetails", new[] { "MachineStockId" });
            DropIndex("dbo.MachineStockDetails", new[] { "ProductId" });
            DropIndex("dbo.MachineStock", new[] { "MachineId" });
            DropIndex("dbo.Machine", new[] { "StoreId" });
            DropIndex("dbo.Video", new[] { "AdvertisementId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.UserRights");
            DropTable("dbo.User");
            DropTable("dbo.Transaction");
            DropTable("dbo.SysMenus");
            DropTable("dbo.SysButton");
            DropTable("dbo.Stock");
            DropTable("dbo.Role");
            DropTable("dbo.PayMent");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Order");
            DropTable("dbo.ProductImage");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.MachineStockDetails");
            DropTable("dbo.MachineStock");
            DropTable("dbo.Store");
            DropTable("dbo.Machine");
            DropTable("dbo.Log");
            DropTable("dbo.InventoryChangeLog");
            DropTable("dbo.Video");
            DropTable("dbo.Advertisement");
        }
    }
}
