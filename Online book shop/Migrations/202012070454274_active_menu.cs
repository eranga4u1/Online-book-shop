namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class active_menu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActiveMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControllerName = c.String(),
                        ActiveClassName = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActiveMenus");
        }
    }
}
