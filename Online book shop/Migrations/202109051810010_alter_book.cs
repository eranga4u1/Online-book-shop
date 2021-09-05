namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_book : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ItemType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "ItemType");
        }
    }
}
