namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_itempack_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPacks", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPacks", "Price");
        }
    }
}
