namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_voucher_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vouchers", "VoucherAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Vouchers", "OrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vouchers", "OrderId");
            DropColumn("dbo.Vouchers", "VoucherAmount");
        }
    }
}
