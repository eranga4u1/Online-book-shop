namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_book_publisher_author : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "FriendlyName", c => c.String());
            AddColumn("dbo.Books", "FriendlyName", c => c.String());
            AddColumn("dbo.Publishers", "FriendlyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "FriendlyName");
            DropColumn("dbo.Books", "FriendlyName");
            DropColumn("dbo.Authors", "FriendlyName");
        }
    }
}
