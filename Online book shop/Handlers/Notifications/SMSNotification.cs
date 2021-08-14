using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Notifications
{
    public class SMSNotifications : INotification
    {
        public bool AccountCreationNotification(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public bool BookOutOfStockRemovedFromCartNotification(Cart c, Book book)
        {
            throw new NotImplementedException();
        }

        public bool BookStockAvailableNotification(Book book)
        {
            throw new NotImplementedException();
        }

        public bool CartStatusUpdate(Cart cart)
        {
            throw new NotImplementedException();
        }

        public bool EmailInvoice(ApplicationUser user, Order order, Cart cart)
        {
            throw new NotImplementedException();
        }

        public void NotifyAllCustomers()
        {
            throw new NotImplementedException();
        }

        public void NotifyGroup()
        {
            throw new NotImplementedException();
        }

        public void NotifySingleCustomer()
        {
            throw new NotImplementedException();
        }

        public bool PurchasedNotification(Cart c)
        {
            throw new NotImplementedException();
        }
    }
}