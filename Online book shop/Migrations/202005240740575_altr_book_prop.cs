namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class altr_book_prop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookProperties", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookProperties", "Price");
        }
    }
}
