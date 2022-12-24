using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Email;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Notifications
{
    public class EmailNotifications : INotification
    {
        public bool AccountCreationNotification(ApplicationUser user)
        {
            String[] paras = new String[1];
            paras[0] = user.Email;
            string content= EmailHandler.SetEmailParameter("UserRegisterConfirmed.html", paras);
           return EmailHandler.Email(content, "noreply@musesbooks.com", user.Email, "Successfully Registered","MusesBooks.com : Successfully Registered")== "Done"?true:false;
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
            String[] paras = new String[2];
            paras[0] = user.Email;
            paras[1] = "http://musesbooks.com/Invoice/Find?uid="+order.UId;
            string content = EmailHandler.SetEmailParameter("Invoice.html", paras);
            return (EmailHandler.Email(EmailHandler.SetEmailParameter("Invoice.html", paras), "noreply@musesbooks.com", user.Email, "Muses Books : Invoice", "ORDER CONFIRMED: MusesBooks.com Invoice for Order no. "+ order.UId) == "Done")?true:false;
            //return EmailHandler.SendMailWithAttachment(content, "noreply@musesbooks.com", user.Email, "Muses Books : Invoice", "\\Content\\UploadFiles\\Invoices", "Invoice_" + order.UId + "_" + order.CartId + ".pdf") == "Done" ? true : false;

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