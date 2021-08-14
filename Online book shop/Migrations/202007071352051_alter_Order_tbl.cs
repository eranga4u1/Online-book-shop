namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_Order_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentMethod", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentMethod");
        }
    }
}
