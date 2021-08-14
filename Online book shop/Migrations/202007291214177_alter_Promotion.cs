namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_Promotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "PromotionMediaId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotions", "PromotionMediaId");
        }
    }
}
