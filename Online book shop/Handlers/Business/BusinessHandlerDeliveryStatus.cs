using Online_book_shop.Handlers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerDeliveryStatus
    {
        public static List<DeliverStatus> GetAllActiveDeliverStatus()
        {
            return DBHandlerDeliveryStatus.GetAllActiveDeliverStatus();
        }
        public static DeliverStatus GetDeliverStatus(int Id)
        {
            return DBHandlerDeliveryStatus.GetDeliverStatus(Id);

        }
        public static DeliverStatus GetDeliverStatusByTitle(string Title)
        {
            return DBHandlerDeliveryStatus.GetDeliverStatusByTitle(Title);
        }

        internal static bool Add(DeliverStatus model)
        {
            return DBHandlerDeliveryStatus.Add(model);
        }

        internal static bool Update(DeliverStatus model)
        {
            return DBHandlerDeliveryStatus.Update(model);
        }
        internal static bool IsAssigned(int Id)
        {
            return DBHandlerDeliveryStatus.IsAssigned(Id);
        }

        internal static bool Delete(int id)
        {
            return DBHandlerDeliveryStatus.Delete(id);
        }
    }
}