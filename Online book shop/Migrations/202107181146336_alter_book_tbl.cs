namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_book_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "MaximumItemPerOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "MaximumItemPerOrder");
        }
    }
}
