namespace Tiplr.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        InventoryId = c.Int(nullable: false, identity: true),
                        InventoryDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Finalized = c.Boolean(nullable: false),
                        CreatedByUser = c.String(maxLength: 128),
                        LastModUser = c.String(maxLength: 128),
                        TotalOnHandValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastModifiedDtTm = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.InventoryId)
                .ForeignKey("dbo.ApplicationUser", t => t.CreatedByUser)
                .ForeignKey("dbo.ApplicationUser", t => t.LastModUser)
                .Index(t => t.CreatedByUser)
                .Index(t => t.LastModUser);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.InventoryItem",
                c => new
                    {
                        InventoryItemId = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OnHandCount = c.Double(nullable: false),
                        LastModifiedDtTm = c.DateTimeOffset(nullable: false, precision: 7),
                        LastModifiedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InventoryItemId)
                .ForeignKey("dbo.Inventory", t => t.InventoryId)
                .ForeignKey("dbo.ApplicationUser", t => t.LastModifiedById)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => t.InventoryId)
                .Index(t => t.ProductId)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        ProductDescription = c.String(maxLength: 150),
                        CategoryId = c.Int(),
                        CountBy = c.String(nullable: false),
                        OrderBy = c.String(nullable: false),
                        UnitsPerPack = c.Int(nullable: false),
                        CasePackPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Par = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedDtTm = c.DateTimeOffset(nullable: false, precision: 7),
                        LastModifiedDtTm = c.DateTimeOffset(nullable: false, precision: 7),
                        InactiveDtTm = c.DateTimeOffset(precision: 7),
                        LastUpdateById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.ApplicationUser", t => t.LastUpdateById)
                .ForeignKey("dbo.ProductCategory", t => t.CategoryId)
                .Index(t => t.CategoryId)
                .Index(t => t.LastUpdateById);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        OrderItemId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        InventoryItemId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        OrderAmt = c.Int(nullable: false),
                        AmtReceived = c.Int(nullable: false),
                        OrderItemTotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderItemId)
                .ForeignKey("dbo.InventoryItem", t => t.InventoryItemId)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.InventoryItemId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        OrderCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderStatusId = c.Int(nullable: false),
                        OrderDate = c.DateTimeOffset(nullable: false, precision: 7),
                        LastModifiedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Inventory", t => t.InventoryId)
                .ForeignKey("dbo.ApplicationUser", t => t.LastModifiedById)
                .ForeignKey("dbo.OrderStatus", t => t.OrderStatusId)
                .Index(t => t.InventoryId)
                .Index(t => t.OrderStatusId)
                .Index(t => t.LastModifiedById);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        OrderStatusId = c.Int(nullable: false, identity: true),
                        OrderStatusMeaning = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderStatusId);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.OrderItem", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderItem", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "OrderStatusId", "dbo.OrderStatus");
            DropForeignKey("dbo.Order", "LastModifiedById", "dbo.ApplicationUser");
            DropForeignKey("dbo.Order", "InventoryId", "dbo.Inventory");
            DropForeignKey("dbo.OrderItem", "InventoryItemId", "dbo.InventoryItem");
            DropForeignKey("dbo.InventoryItem", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "CategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.Product", "LastUpdateById", "dbo.ApplicationUser");
            DropForeignKey("dbo.InventoryItem", "LastModifiedById", "dbo.ApplicationUser");
            DropForeignKey("dbo.InventoryItem", "InventoryId", "dbo.Inventory");
            DropForeignKey("dbo.Inventory", "LastModUser", "dbo.ApplicationUser");
            DropForeignKey("dbo.Inventory", "CreatedByUser", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropIndex("dbo.Order", new[] { "LastModifiedById" });
            DropIndex("dbo.Order", new[] { "OrderStatusId" });
            DropIndex("dbo.Order", new[] { "InventoryId" });
            DropIndex("dbo.OrderItem", new[] { "OrderId" });
            DropIndex("dbo.OrderItem", new[] { "InventoryItemId" });
            DropIndex("dbo.OrderItem", new[] { "ProductId" });
            DropIndex("dbo.Product", new[] { "LastUpdateById" });
            DropIndex("dbo.Product", new[] { "CategoryId" });
            DropIndex("dbo.InventoryItem", new[] { "LastModifiedById" });
            DropIndex("dbo.InventoryItem", new[] { "ProductId" });
            DropIndex("dbo.InventoryItem", new[] { "InventoryId" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Inventory", new[] { "LastModUser" });
            DropIndex("dbo.Inventory", new[] { "CreatedByUser" });
            DropTable("dbo.IdentityRole");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.Order");
            DropTable("dbo.OrderItem");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.InventoryItem");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.Inventory");
        }
    }
}
