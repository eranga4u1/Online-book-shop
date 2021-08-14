using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerDelivery
    {
        public static Order Post(Order order)
        {
            string id = HTMLHelper.GenarateRandomRef(order.CartId);//Guid.NewGuid();
            order.CreatedDate = DateTime.UtcNow;
            order.UpdatedDate = DateTime.UtcNow;
            order.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            order.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            order.UId =(!string.IsNullOrEmpty(order.UId))?(order.UId):id;
            order.PaymentStatus = (int)PaymentStatus.PendingPayment;
                DeliverStatus tempCartDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("temp_cart");

            order.DeliveryStatus = tempCartDeliverStatuses.Id;

           // order.BillingAddress = order.DeliveryAddress;
           
            
            //DeliveryCharge dCharge = BusinessHandlerDeliveryCharges.GetDeliveryChargeById(order.AreaId);
            //if (dCharge !=null)
            //{
            //    order.DeliveryCharges = dCharge.Amount;
            //}
            //else
            //{
            //    order.DeliveryCharges = 100000;
            //}
            
            return (!string.IsNullOrEmpty(order.UId)? DBHandlerDelivery.Update(order):DBHandlerDelivery.Post(order));
        }
        public static Order Get(int Id)
        {
            return DBHandlerDelivery.GetById(Id);
        }
        public static Order Get(string Id)
        {
            return DBHandlerDelivery.GetById(Id);
        }

        internal static bool ChangePaymentStatus(PaymentStatus paymentStatus, int orderId)
        {
            return DBHandlerDelivery.ChangePaymentStatus(paymentStatus, orderId);
        }

        internal static bool ChangeDeliveryStatus(DeliverStatus deliveryStatus, int orderId)
        {
            return DBHandlerDelivery.ChangeDeliveryStatus(deliveryStatus, orderId);
        }
      
    }
}