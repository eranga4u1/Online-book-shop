namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_cart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "VoucherCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "VoucherCode");
        }
    }
}
