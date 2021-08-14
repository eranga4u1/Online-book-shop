namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_user_tbl_address2021_02_08 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "isDefaultAddress", c => c.Boolean(nullable: false));
            AddColumn("dbo.Addresses", "isBillingAddress", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "NickName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NickName");
            DropColumn("dbo.Addresses", "isBillingAddress");
            DropColumn("dbo.Addresses", "isDefaultAddress");
        }
    }
}
