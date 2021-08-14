namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_bookpacks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPacks", "NumberOfPacks", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPacks", "NumberOfPacks");
        }
    }
}
