namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_addrss_tbl2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Addresses", "CreatedBy", c => c.String());
            AddColumn("dbo.Addresses", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Addresses", "UpdatedBy", c => c.String());
            AddColumn("dbo.Addresses", "isDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "isDeleted");
            DropColumn("dbo.Addresses", "UpdatedBy");
            DropColumn("dbo.Addresses", "UpdatedDate");
            DropColumn("dbo.Addresses", "CreatedBy");
            DropColumn("dbo.Addresses", "CreatedDate");
        }
    }
}
