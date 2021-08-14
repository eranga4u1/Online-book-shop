namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Email", c => c.String());
            AddColumn("dbo.Authors", "ContactNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "ContactNumber");
            DropColumn("dbo.Authors", "Email");
        }
    }
}
