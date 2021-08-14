namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_order_tbl_21_01_24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "BillingAddressId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "DeliveryAddressId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "PaymentSpecialNote", c => c.String());
            AddColumn("dbo.Orders", "DeliverySpecialNote", c => c.String());
            DropColumn("dbo.Orders", "SpecialNote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "SpecialNote", c => c.String());
            DropColumn("dbo.Orders", "DeliverySpecialNote");
            DropColumn("dbo.Orders", "PaymentSpecialNote");
            DropColumn("dbo.Orders", "DeliveryAddressId");
            DropColumn("dbo.Orders", "BillingAddressId");
        }
    }
}
