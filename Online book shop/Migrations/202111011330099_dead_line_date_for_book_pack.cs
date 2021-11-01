namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dead_line_date_for_book_pack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "AvailableUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "AvailableUntil");
        }
    }
}
