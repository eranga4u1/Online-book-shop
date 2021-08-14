namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemMedia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectId = c.Int(nullable: false),
                        ObjectType = c.Int(nullable: false),
                        MediaId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ItemPacks", "FrontCoverMediaId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPacks", "FrontCoverMediaId");
            DropTable("dbo.ItemMedias");
        }
    }
}
