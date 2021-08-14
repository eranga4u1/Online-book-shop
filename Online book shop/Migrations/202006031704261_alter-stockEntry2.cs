namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterstockEntry2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockEntries", "BookPrpertyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockEntries", "BookPrpertyId");
        }
    }
}
