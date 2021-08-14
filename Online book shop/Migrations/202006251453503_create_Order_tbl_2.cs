namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_Order_tbl_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FirstName", c => c.String());
            AddColumn("dbo.Orders", "LastName", c => c.String());
            AddColumn("dbo.Orders", "EmailAddress", c => c.String());
            AddColumn("dbo.Orders", "DeliveryMethod", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "AreaId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "DeliveryCharges", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DeliveryCharges");
            DropColumn("dbo.Orders", "AreaId");
            DropColumn("dbo.Orders", "DeliveryMethod");
            DropColumn("dbo.Orders", "EmailAddress");
            DropColumn("dbo.Orders", "LastName");
            DropColumn("dbo.Orders", "FirstName");
        }
    }
}
