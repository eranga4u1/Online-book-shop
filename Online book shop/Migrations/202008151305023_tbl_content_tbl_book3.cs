namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_content_tbl_book3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "PreReleaseEndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "PreReleaseEndDate", c => c.DateTime());
        }
    }
}
