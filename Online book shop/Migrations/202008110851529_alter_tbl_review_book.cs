namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_review_book : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "RelaseDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "RelaseDate");
        }
    }
}
