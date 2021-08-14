namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_DeliveryCharge_20210129 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryCharges", "SliceByGrams", c => c.Int(nullable: false));
            AddColumn("dbo.DeliveryCharges", "UnitPricePerSlice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.DeliveryCharges", "isDynamic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeliveryCharges", "isDynamic");
            DropColumn("dbo.DeliveryCharges", "UnitPricePerSlice");
            DropColumn("dbo.DeliveryCharges", "SliceByGrams");
        }
    }
}
