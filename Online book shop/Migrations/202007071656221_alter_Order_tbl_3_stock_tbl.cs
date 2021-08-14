namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_Order_tbl_3_stock_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentStatus", c => c.Int(nullable: false));
            AddColumn("dbo.StockEntries", "Details", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockEntries", "Details");
            DropColumn("dbo.Orders", "PaymentStatus");
        }
    }
}
