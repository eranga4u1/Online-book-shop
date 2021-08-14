namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartBook_Altered : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cart_Book", "SpecialNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cart_Book", "SpecialNote");
        }
    }
}
