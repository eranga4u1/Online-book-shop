namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_delivery_charge20210217 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryCharges", "isDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeliveryCharges", "isDeleted");
        }
    }
}
