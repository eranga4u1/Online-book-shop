using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_book_shop.Handlers.Notifications
{
    interface INotification
    {
        void NotifyAllCustomers();
        void NotifySingleCustomer();
        void NotifyGroup();
        bool AccountCreationNotification(ApplicationUser user);
        bool PurchasedNotification(Cart c);
        bool BookOutOfStockRemovedFromCartNotification(Cart c,Book book);
        bool BookStockAvailableNotification(Book book);
        bool CartStatusUpdate(Cart cart);
        bool EmailInvoice(ApplicationUser user,Order order, Cart cart);

    }
}
