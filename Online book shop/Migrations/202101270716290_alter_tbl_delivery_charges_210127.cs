namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_tbl_delivery_charges_210127 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeliveryCharges", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeliveryCharges", "Country");
        }
    }
}
