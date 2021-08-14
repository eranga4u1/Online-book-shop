namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes4 : DbMigration
    {
        public override void Up()
        {
           // DropColumn("dbo.UserReviews", "Comment");
        }
        
        public override void Down()
        {
           // AddColumn("dbo.UserReviews", "Comment", c => c.String());
        }
    }
}
