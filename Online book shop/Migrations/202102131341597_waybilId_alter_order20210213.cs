namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class waybilId_alter_order20210213 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "WaybillId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "WaybillId");
        }
    }
}
