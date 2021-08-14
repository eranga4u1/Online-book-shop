namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_book_book_pack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "YoutubeUrl", c => c.String());
            AddColumn("dbo.ItemPacks", "YoutubeUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPacks", "YoutubeUrl");
            DropColumn("dbo.Books", "YoutubeUrl");
        }
    }
}
