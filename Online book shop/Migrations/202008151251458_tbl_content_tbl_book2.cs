namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_content_tbl_book2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "PreReleaseEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "PreReleaseEndDate", c => c.DateTime(nullable: false));
        }
    }
}
