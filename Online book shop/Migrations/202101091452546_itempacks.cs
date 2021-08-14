namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itempacks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemPack_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemPackId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        ItemPropertyId = c.Int(nullable: false),
                        NumberOfItems = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ItemPacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        LocalTitle = c.String(),
                        ItemPackIdentityID = c.String(),
                        Description = c.String(),
                        SaleType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        SaleStartDate = c.DateTime(nullable: false),
                        SaleEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ItemPacks");
            DropTable("dbo.ItemPack_Item");
        }
    }
}
