namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterstockEntry : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StockEntries", "UpdatedDate");
            DropColumn("dbo.StockEntries", "isDeleted");
            DropColumn("dbo.StockEntries", "UpdatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockEntries", "UpdatedBy", c => c.String());
            AddColumn("dbo.StockEntries", "isDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StockEntries", "UpdatedDate", c => c.DateTime(nullable: false));
        }
    }
}
