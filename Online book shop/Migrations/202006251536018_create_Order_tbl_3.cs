namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_Order_tbl_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "CreatedBy", c => c.String());
            AddColumn("dbo.Orders", "UpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "UpdatedBy");
            DropColumn("dbo.Orders", "CreatedBy");
            DropColumn("dbo.Orders", "UpdatedDate");
            DropColumn("dbo.Orders", "CreatedDate");
        }
    }
}
