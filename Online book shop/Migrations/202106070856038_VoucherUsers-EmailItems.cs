namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoucherUsersEmailItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                        Subject = c.String(),
                        AttachmentFile = c.String(),
                        FileName = c.String(),
                        EmailStatus = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoucherUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VoucherId = c.Int(nullable: false),
                        UserId = c.String(),
                        OrderId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VoucherUsers");
            DropTable("dbo.EmailItems");
        }
    }
}
