namespace Tiplr.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updtDataTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryItem", "OnHandCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryItem", "OnHandCount", c => c.Double(nullable: false));
        }
    }
}
