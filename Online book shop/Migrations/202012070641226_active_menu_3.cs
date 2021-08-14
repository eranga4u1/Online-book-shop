namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class active_menu_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActiveMenus", "MethodName", c => c.String());
            DropColumn("dbo.ActiveMenus", "ActiveClassName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActiveMenus", "ActiveClassName", c => c.String());
            DropColumn("dbo.ActiveMenus", "MethodName");
        }
    }
}
