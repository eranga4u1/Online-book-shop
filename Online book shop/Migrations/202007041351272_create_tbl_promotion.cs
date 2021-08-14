namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_tbl_promotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionTitle = c.String(),
                        PromotionDescription = c.String(),
                        PromotionTypesFor = c.Int(nullable: false),
                        PromotionMethods = c.Int(nullable: false),
                        ObjectType = c.Int(nullable: false),
                        ObjectId = c.Int(nullable: false),
                        DiscountValue = c.Double(nullable: false),
                        OtherParameters = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Promotions");
        }
    }
}
