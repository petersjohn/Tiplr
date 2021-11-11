namespace Tiplr.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedOrderedIndToInvItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryItem", "OrderedInd", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InventoryItem", "OrderedInd");
        }
    }
}
