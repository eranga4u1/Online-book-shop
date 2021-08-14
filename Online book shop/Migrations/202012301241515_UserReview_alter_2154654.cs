namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReview_alter_2154654 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "isspolier", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserReviews", "isanonymous", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "isanonymous");
            DropColumn("dbo.UserReviews", "isspolier");
        }
    }
}
