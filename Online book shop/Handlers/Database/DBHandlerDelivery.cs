using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerDelivery
    {
        internal static Order Post(Order order)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Orders.Add(order);
                    if (ctx.SaveChanges()>0)
                    {
                        return order;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Order GetById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                 return ctx.Orders.Where(x=> x.Id==id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Order GetById(string id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Orders.Where(x => x.UId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Order Update(Order order)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Order Obj = ctx.Orders.Where(x => x.UId == order.UId).FirstOrDefault();
                    if(Obj != null)
                    {
                        Obj.FirstName = order.FirstName;
                        Obj.LastName = order.LastName;
                        Obj.BillingAddressId = order.BillingAddressId;
                        Obj.DeliveryAddressId = order.DeliveryAddressId;
                        Obj.BillingAddress = order.BillingAddress;
                        Obj.DeliveryAddress = order.DeliveryAddress;
                        Obj.DeliveryMethod = order.DeliveryMethod;
                        Obj.PaymentMethod = order.PaymentMethod;
                        Obj.PaymentSpecialNote = order.PaymentSpecialNote;
                        Obj.DeliveryCharges = order.DeliveryCharges;
                        Obj.DeliverySpecialNote = order.DeliverySpecialNote;
                        Obj.CartId = order.CartId;
                        ctx.SaveChanges();
                        //if (ctx.SaveChanges() > 0)
                        //{
                            return order;
                        //}
                    }
                    else
                    {
                        ctx.Orders.Add(order);
                        if (ctx.SaveChanges() > 0)
                        {
                            return order;
                        }
                    }
                    
                    
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool ChangePaymentStatus(PaymentStatus paymentStatus, int orderId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   var order= ctx.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                    if(order != null)
                    {
                        order.PaymentStatus = (int)paymentStatus;
                        order.UpdatedDate = DateTime.UtcNow;
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool ChangeDeliveryStatus(DeliverStatus deliveryStatus, int orderId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var order = ctx.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                    if (order != null)
                    {
                        order.DeliveryStatus = deliveryStatus.Id;
                        order.UpdatedDate = DateTime.UtcNow;
                        BusinessHandlerMPLog.Log(LogType.StatusChanged, "DBHandlerDelivery", "ChangeDeliveryStatus", string.Format("OrderId:{0},Status:{1}", orderId.ToString(), deliveryStatus.Id.ToString()));
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}