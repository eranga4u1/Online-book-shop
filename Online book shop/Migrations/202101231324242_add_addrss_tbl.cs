namespace Online_book_shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_addrss_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        isPublic = c.Boolean(nullable: false),
                        RefId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Company = c.String(),
                        AddressLine01 = c.String(),
                        AddressLine02 = c.String(),
                        AddressLine03 = c.String(),
                        ContactNumber1 = c.String(),
                        ContactNumber2 = c.String(),
                        EmailAddress = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        District = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Addresses");
        }
    }
}
