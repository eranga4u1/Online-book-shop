namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_cartbook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cart_Book", "BookPropertyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cart_Book", "BookPropertyId");
        }
    }
}
