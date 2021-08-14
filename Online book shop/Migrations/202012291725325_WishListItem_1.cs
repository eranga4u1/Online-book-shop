namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WishListItem_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        UId = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserReviews", "isApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "isApproved");
            DropTable("dbo.WishListItems");
        }
    }
}
