namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tbl_saletypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DisplayText = c.String(),
                        BackGroundColor = c.String(),
                        ForeColor = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SaleStatus");
        }
    }
}
