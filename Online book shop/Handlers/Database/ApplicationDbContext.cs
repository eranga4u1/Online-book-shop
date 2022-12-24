using Microsoft.AspNet.Identity.EntityFramework;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Book_Author> Book_Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookProperties> BookProperties { get; set; }
        public DbSet<Book_Category> Book_Categories { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Book> Cart_Books { get; set; }
        public DbSet<StockEntry> StockEntries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<DeliveryCharge> DeliveryCharges { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<ActiveMenu> ActiveMenus { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<ItemPack> ItemPacks { get; set; }
        public DbSet<ItemPack_Item> ItemPack_Items { get; set; }
        public DbSet<ItemMedia> ItemMedias { get; set; }
        public DbSet<Address> Addreses { get; set; }
        public DbSet<SaleStatus> SaleStatus { get; set; }
        public DbSet<DeliverStatus> DeliverStatuses { get; set; }
        public DbSet<MPLog> MPLogs { get; set; }
        public DbSet<RecentyViewItems> RecentyViewItems { get; set; }
        public DbSet<VoucherUser> VoucherUsers { get; set; }
        public DbSet<EmailItem> EmailItems { get; set; }
        public DbSet<PaymentState> PaymentStatus { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<EmployeeMaster>()
        //        .MapToStoredProcedures(s => s.Insert(u => u.HasName("InsertEmployee", "dbo"))
        //                                        .Update(u => u.HasName("UpdateEmployee", "dbo"))
        //                                        .Delete(u => u.HasName("DeleteEmployee", "dbo"))
        //        );
        //}
    }
}