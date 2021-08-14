namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_addrss_tbl1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "UserId", c => c.Int(nullable: false));
        }
    }
}
