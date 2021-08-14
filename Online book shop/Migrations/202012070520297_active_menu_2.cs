namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class active_menu_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActiveMenus", "ActiveMenuOrderNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActiveMenus", "ActiveMenuOrderNo");
        }
    }
}
