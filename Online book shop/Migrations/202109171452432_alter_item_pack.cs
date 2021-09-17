namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_item_pack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemPack_Item", "isDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ItemPack_Item", "DeletedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemPack_Item", "DeletedDate");
            DropColumn("dbo.ItemPack_Item", "isDeleted");
        }
    }
}
