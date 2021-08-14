namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_bk_property : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookProperties", "Title", c => c.String());
            AddColumn("dbo.BookProperties", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookProperties", "Description");
            DropColumn("dbo.BookProperties", "Title");
        }
    }
}
