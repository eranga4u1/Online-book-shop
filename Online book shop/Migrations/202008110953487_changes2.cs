namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "UserComment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "UserComment");
        }
    }
}
