namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class book_bookProperty_tbl_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book_Category", "BookPropertyId", c => c.Int(nullable: false));
            AddColumn("dbo.BookProperties", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.BookProperties", "WeightByGrams", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Books", "PublisherId", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "SaleType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "SaleType");
            DropColumn("dbo.Books", "PublisherId");
            DropColumn("dbo.BookProperties", "WeightByGrams");
            DropColumn("dbo.BookProperties", "LanguageId");
            DropColumn("dbo.Book_Category", "BookPropertyId");
        }
    }
}
