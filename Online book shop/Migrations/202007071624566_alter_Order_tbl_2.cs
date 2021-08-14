namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_Order_tbl_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "UId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "UId");
        }
    }
}
