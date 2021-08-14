using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerPublisher
    {
        public static bool SaveNewPublisher(Publisher publisher)
        {
            publisher.FriendlyName=HTMLHelper.RemoveSpecialCharacters(publisher.Name);
            if (DBHandlerPublisher.SavePublisher(publisher) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<Publisher> GetPublishers()
        {
            return DBHandlerPublisher.GetPublishers();
        }
        public static Publisher GetPublisherById(int id)
        {
            return DBHandlerPublisher.GetPublisher(id);
        }
        public static Publisher UpdatePublisher(Publisher publisher)
        {
            publisher.FriendlyName = HTMLHelper.RemoveSpecialCharacters(publisher.Name);
            return DBHandlerPublisher.UpdatePublisher(publisher);
        }
        internal static bool Show(int id)
        {
            return DBHandlerPublisher.ShowHide(id, false);
        }
        internal static bool Hide(int id)
        {
            return DBHandlerPublisher.ShowHide(id, true);
        }
    }
}