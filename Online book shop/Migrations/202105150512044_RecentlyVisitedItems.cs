namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecentlyVisitedItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecentyViewItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        RecentlyVisitedItems = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RecentyViewItems");
        }
    }
}
