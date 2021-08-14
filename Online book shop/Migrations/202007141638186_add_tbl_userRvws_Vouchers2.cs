namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tbl_userRvws_Vouchers2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "ObjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "ObjectId");
        }
    }
}
