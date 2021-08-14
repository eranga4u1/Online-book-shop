namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_Article621863 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Url");
        }
    }
}
