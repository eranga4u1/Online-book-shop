namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Media", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Media", "FileName");
        }
    }
}
