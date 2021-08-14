namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_deliveryCharges_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeliveryCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeliveryType = c.Int(nullable: false),
                        StartWeightByGrams = c.Int(nullable: false),
                        EndWeightByGrams = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Area = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeliveryCharges");
        }
    }
}
