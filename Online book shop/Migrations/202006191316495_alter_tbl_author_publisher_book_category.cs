namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_author_publisher_book_category : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "LocalName", c => c.String());
            AddColumn("dbo.Books", "LocalTitle", c => c.String());
            AddColumn("dbo.Books", "Ratings", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "LocalCategoryName", c => c.String());
            AddColumn("dbo.Publishers", "LocalName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "LocalName");
            DropColumn("dbo.Categories", "LocalCategoryName");
            DropColumn("dbo.Books", "Ratings");
            DropColumn("dbo.Books", "LocalTitle");
            DropColumn("dbo.Authors", "LocalName");
        }
    }
}
