using OfficeOpenXml.Style;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerOrder
    {
        internal static List<Order> Get(int pageId, int itemPerPage, int deliveryStatus, int paymentStatus)
        {
            return DBHandlerOrder.Get(pageId, itemPerPage, deliveryStatus, paymentStatus);
        }

        internal static Order  GetByUID(string uid)
        {
            return DBHandlerOrder.GetByUID(uid);
        }

        internal static int GetPageCount(int pageId, int itemPerPage, int deliveryStatus, int paymentStatus)
        {
            return DBHandlerOrder.GetPageCount(pageId, itemPerPage, deliveryStatus, paymentStatus);

        }

        internal static Order Get(int id)
        {
            return DBHandlerOrder.Get(id);
        }

        internal static bool ChangeStatus(int orderId, int statusId,string trackingId)
        {
            return DBHandlerOrder.ChangeStatus(orderId, statusId, trackingId);
        }
        internal static bool ChangePaymentStatus(int orderId, int statusId, string note)
        {
            return DBHandlerOrder.ChangePaymentStatus(orderId, statusId, note);
        }

        internal static List<Order> GetFiltered(int deliveryStatus, int orderType, string startDate, string endDate)
        {
            DateTime dateTime_start = DateTimeOffset.Parse("2021-04-01").UtcDateTime;
            DateTime dateTime_end = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(startDate))
            {
                dateTime_start = DateTimeOffset.Parse(startDate).UtcDateTime;
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                dateTime_end = DateTimeOffset.Parse(endDate).UtcDateTime;
            }
            return DBHandlerOrder.GetFiltered(deliveryStatus, orderType, dateTime_start, dateTime_end);
        }

        internal static List<Order> GerOrderForLogingUser()
        {
            return DBHandlerOrder.GetOrderByUser(BusinessHandlerAuthor.GetLoginUserId());
        }

        internal static bool ChangeStatus(List<Order> orders)
        {
            return DBHandlerOrder.ChangeStatus(orders);
        }
        internal static bool ChangePaymentStatus(List<Order> orders)
        {
            return DBHandlerOrder.ChangePaymentStatus(orders);
        }

        internal static decimal UpdateKokoServiceCharge(int OrderId)
        {
            return DBHandlerOrder.UpdateKokoServiceCharge(OrderId);
        }
    }
}