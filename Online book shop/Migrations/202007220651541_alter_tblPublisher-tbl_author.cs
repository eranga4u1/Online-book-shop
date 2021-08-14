namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tblPublishertbl_author : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "CoverPictureMediaId", c => c.Int(nullable: false));
            AddColumn("dbo.Publishers", "CoverPictureMediaId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "CoverPictureMediaId");
            DropColumn("dbo.Authors", "CoverPictureMediaId");
        }
    }
}
