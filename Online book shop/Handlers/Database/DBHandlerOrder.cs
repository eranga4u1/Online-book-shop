using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerOrder
    {
        internal static List<Order> Get(int pageId, int itemPerPage, int deliveryStatus, int paymentStatus)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if(deliveryStatus ==-1 && paymentStatus == -1)
                    {
                      var rslts=  ctx.Orders.Where(x=> x.DeliveryStatus>0).OrderByDescending(o=> o.Id).Skip((pageId - 1) * itemPerPage).Take(itemPerPage);
                        return rslts!=null?rslts.ToList():null;
                    }
                    else
                    {
                        var rslts = ctx.Orders.Where(x => x.DeliveryStatus == deliveryStatus).OrderByDescending(o => o.Id).Skip((pageId - 1) * itemPerPage).Take(itemPerPage);
                        // var rslts=   ctx.Orders.Where(x => x.DeliveryStatus == deliveryStatus ).Skip((pageId - 1) * itemPerPage).Take(itemPerPage);
                        return rslts != null ? rslts.ToList() : null;
                    }
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool UpdateOrderDescription()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    
                   DeliverStatus tempCart = ctx.DeliverStatuses.Where(x => x.Title == "temp_cart").FirstOrDefault();
                  //DeliverStatus confirmedOrder =  ctx.DeliverStatuses.Where(x => x.Title == "confirmed_order").FirstOrDefault();
                  List<Order> order_list= ctx.Orders.Where(x => x.DeliveryStatus != tempCart.Id && string.IsNullOrEmpty(x.DeliverySpecialNote)).ToList();
                    if(order_list !=null && order_list.Count > 0)
                    {
                        foreach(Order order in order_list)
                        {
                            order.DeliverySpecialNote= BusinessHandlerReport.GetOrderDescription(order.CartId);
                        }
                        if (ctx.SaveChanges() > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerOrder", "UpdateOrderDescription");
            }
            return false;
        }

        internal static List<Order> GetFiltered( int deliveryStatus, int orderType, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if(deliveryStatus==-1 && orderType == -1)
                    {
                      return  ctx.Orders.Where(x => x.CreatedDate > startDate && x.CreatedDate < endDate).OrderBy(x => x.CreatedDate).ToList();
                    }
                    else if(deliveryStatus == -1)
                    {
                        if (orderType == (int)OrderType.PreOrder)
                        {
                            List<int> preOrderCarts = ctx.Cart_Books.Where(x => x.SpecialNote == "(Preordered book)" && x.CreatedDate > startDate && x.CreatedDate < endDate).Select(x => x.CartId).ToList();
                            return ctx.Orders.Where(x =>preOrderCarts.Contains(x.CartId)).OrderBy(x => x.CreatedDate).ToList();
                        }
                        else
                        {
                            List<int> allCarts = ctx.Cart_Books.Where(x => x.CreatedDate > startDate && x.CreatedDate < endDate).Select(x => x.CartId).ToList();
                            List<int> preOrderCarts = ctx.Cart_Books.Where(x => x.SpecialNote == "(Preordered book)" && x.CreatedDate > startDate && x.CreatedDate < endDate).Select(x => x.CartId).ToList();
                            List<int> FilteredList = allCarts.Where(p => preOrderCarts.All(p2 => p2 != p)).ToList();

                            return ctx.Orders.Where(x =>FilteredList.Contains(x.CartId)).OrderBy(x => x.CreatedDate).ToList();
                        }
                    }
                    else if (orderType == -1)
                    {
                        return ctx.Orders.Where(x => x.DeliveryStatus == deliveryStatus && x.CreatedDate > startDate && x.CreatedDate < endDate).OrderBy(x => x.CreatedDate).ToList();
                    }
                    else if (orderType == (int)OrderType.PreOrder)
                    {
                        List<int> preOrderCarts= ctx.Cart_Books.Where(x => x.SpecialNote == "(Preordered book)" && x.CreatedDate > startDate && x.CreatedDate < endDate).Select(x => x.CartId).ToList();
                        return  ctx.Orders.Where(x => x.DeliveryStatus == deliveryStatus && preOrderCarts.Contains(x.CartId)).OrderBy(x=> x.CreatedDate).ToList();
                    }
                    else
                    {
                        List<int> allCarts = ctx.Cart_Books.Where(x =>  x.CreatedDate > startDate && x.CreatedDate < endDate).Select(x => x.CartId).ToList();
                        List<int> preOrderCarts = ctx.Cart_Books.Where(x => x.SpecialNote == "(Preordered book)" && x.CreatedDate > startDate && x.CreatedDate < endDate).Select(x => x.CartId).ToList();
                        List<int> FilteredList = allCarts.Where(p => preOrderCarts.All(p2 => p2 != p)).ToList();

                        return ctx.Orders.Where(x => x.DeliveryStatus == deliveryStatus && FilteredList.Contains(x.CartId)).OrderBy(x => x.CreatedDate).ToList();
                    }
                }
            }catch(Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception,ex.Message, "Order", "GetFiltered");
            }
            return null;
        }

        internal static bool ChangeStatus(List<Order> orders)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    foreach(Order obj in orders)
                    {
                        Order order = ctx.Orders.Where(x => x.Id == obj.Id).FirstOrDefault();
                        if (order != null)
                        {
                            order.UpdatedDate = DateTime.UtcNow;
                            order.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                            order.DeliveryStatus = obj.DeliveryStatus;
                            BusinessHandlerMPLog.Log(LogType.StatusChanged, "DBHandlerOrder", "ChangeStatus", string.Format("OrderId:{0},Status:{1}", order.Id.ToString(), obj.DeliveryStatus.ToString()));
                            //if (!string.IsNullOrEmpty(trackingId))
                            //{
                            //    order.WaybillId = trackingId;
                            //}
                            //else
                            //{
                            //    order.WaybillId = null;
                            //}                           
                        }
                       
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static Order GetByUID(string uid)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Orders.Where(x => x.UId== uid).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool ChangeStatus(int orderId, int statusId,string trackingId)
        {

            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Order order = ctx.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                    if (order != null)
                    {
                        order.UpdatedDate = DateTime.UtcNow;
                        order.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        order.DeliveryStatus = statusId;
                        if (!string.IsNullOrEmpty(trackingId)) { 
                            order.WaybillId = trackingId; 
                        } else {
                            order.WaybillId = null;
                                }
                        if (ctx.SaveChanges() > 0)
                        {
                            BusinessHandlerMPLog.Log(LogType.StatusChanged, "DBHandlerOrder", "ChangeStatus", string.Format("OrderId:{0},Status:{1}", orderId.ToString(), statusId.ToString()));
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal static bool ChangePaymentStatus(int orderId, int statusId, string note)
        {

            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    Order order = ctx.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                    if (order != null)
                    {
                        order.UpdatedDate = DateTime.UtcNow;
                        order.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                        order.PaymentStatus = statusId;
                        if (!string.IsNullOrEmpty(note))
                        {
                            order.PaymentSpecialNote = string.Format("{0} Added by admin :{1}", order.PaymentSpecialNote, note);
                        }
                        
                        if (ctx.SaveChanges() > 0)
                        {
                            BusinessHandlerMPLog.Log(LogType.StatusChanged, "DBHandlerOrder", "ChangePaymentStatus", string.Format("OrderId:{0},Status:{1}", orderId.ToString(), statusId.ToString()));
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal static Order Get(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    return ctx.Orders.Where(x => x.Id == id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static int GetPageCount(int pageId, int itemPerPage, int deliveryStatus, int paymentStatus)
        {
            var rslts = 1;
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    if (deliveryStatus == -1 || paymentStatus == -1)
                    {
                         rslts = ctx.Orders.Count();                       
                    }
                    else
                    {
                         rslts = ctx.Orders.Where(x => x.DeliveryStatus == deliveryStatus && x.PaymentStatus == paymentStatus).Count();
                    }
                }
                if (rslts % itemPerPage == 0)
                {
                    return rslts / itemPerPage;
                }
                else
                {
                    return ((rslts - (rslts % itemPerPage)) / itemPerPage) + 1;
                }

            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        internal static List<Order> GetOrderByUser(string uid)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    DeliverStatus status = ctx.DeliverStatuses.Where(x => x.Title == "temp_cart").FirstOrDefault();
                    return ctx.Orders.Where(x => x.CreatedBy==uid && x.DeliveryStatus != status.Id).OrderByDescending(x=> x.CreatedDate).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}