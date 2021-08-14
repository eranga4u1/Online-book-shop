namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mpLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MPLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Message = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        Para = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MPLogs");
        }
    }
}
