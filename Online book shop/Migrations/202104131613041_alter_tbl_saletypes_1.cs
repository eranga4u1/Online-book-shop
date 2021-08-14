namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_saletypes_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleStatus", "isAddToCartEnables", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleStatus", "isAddToCartEnables");
        }
    }
}
