namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_order_tbl_20220201 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderSummary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderSummary");
        }
    }
}
