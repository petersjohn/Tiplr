namespace Tiplr.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.InventoryItem", name: "Id", newName: "LastModifiedById");
            RenameColumn(table: "dbo.Product", name: "Id", newName: "LastUpdateById");
            RenameIndex(table: "dbo.InventoryItem", name: "IX_Id", newName: "IX_LastModifiedById");
            RenameIndex(table: "dbo.Product", name: "IX_Id", newName: "IX_LastUpdateById");
            AddColumn("dbo.Inventory", "LastModUser", c => c.String(maxLength: 128));
            AddColumn("dbo.Inventory", "TotalOnHandValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderItem", "OrderItemTotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Order", "LastModifiedById", c => c.String(maxLength: 128));
            AddColumn("dbo.OrderStatus", "OrderStatusMeaning", c => c.String());
            AlterColumn("dbo.Inventory", "CreatedByUser", c => c.String(maxLength: 128));
            CreateIndex("dbo.Inventory", "CreatedByUser");
            CreateIndex("dbo.Inventory", "LastModUser");
            CreateIndex("dbo.Order", "LastModifiedById");
            AddForeignKey("dbo.Inventory", "CreatedByUser", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.Inventory", "LastModUser", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.Order", "LastModifiedById", "dbo.ApplicationUser", "Id");
            DropColumn("dbo.Inventory", "UpdtUser");
            DropColumn("dbo.Order", "CreatedByUser");
            DropColumn("dbo.Order", "FinalizeUser");
            DropColumn("dbo.OrderStatus", "OrderStatusDisplay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderStatus", "OrderStatusDisplay", c => c.String());
            AddColumn("dbo.Order", "FinalizeUser", c => c.Guid(nullable: false));
            AddColumn("dbo.Order", "CreatedByUser", c => c.Guid(nullable: false));
            AddColumn("dbo.Inventory", "UpdtUser", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Order", "LastModifiedById", "dbo.ApplicationUser");
            DropForeignKey("dbo.Inventory", "LastModUser", "dbo.ApplicationUser");
            DropForeignKey("dbo.Inventory", "CreatedByUser", "dbo.ApplicationUser");
            DropIndex("dbo.Order", new[] { "LastModifiedById" });
            DropIndex("dbo.Inventory", new[] { "LastModUser" });
            DropIndex("dbo.Inventory", new[] { "CreatedByUser" });
            AlterColumn("dbo.Inventory", "CreatedByUser", c => c.Guid(nullable: false));
            DropColumn("dbo.OrderStatus", "OrderStatusMeaning");
            DropColumn("dbo.Order", "LastModifiedById");
            DropColumn("dbo.OrderItem", "OrderItemTotalPrice");
            DropColumn("dbo.Inventory", "TotalOnHandValue");
            DropColumn("dbo.Inventory", "LastModUser");
            RenameIndex(table: "dbo.Product", name: "IX_LastUpdateById", newName: "IX_Id");
            RenameIndex(table: "dbo.InventoryItem", name: "IX_LastModifiedById", newName: "IX_Id");
            RenameColumn(table: "dbo.Product", name: "LastUpdateById", newName: "Id");
            RenameColumn(table: "dbo.InventoryItem", name: "LastModifiedById", newName: "Id");
        }
    }
}
