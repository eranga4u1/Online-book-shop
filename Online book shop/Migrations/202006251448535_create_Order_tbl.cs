namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_Order_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CartId = c.Int(nullable: false),
                        BillingAddress = c.String(),
                        DeliveryAddress = c.String(),
                        ContactNumber = c.String(),
                        SpecialNote = c.String(),
                        DeliveryStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Carts", "CartStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Carts", "DeliveryAddress");
            DropColumn("dbo.Carts", "DeliveryStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carts", "DeliveryStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "DeliveryAddress", c => c.String());
            DropColumn("dbo.Carts", "CartStatus");
            DropTable("dbo.Orders");
        }
    }
}
