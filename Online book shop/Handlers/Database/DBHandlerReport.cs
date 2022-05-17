using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerReport
    {
        internal static List<DeliveryReport> GetTodayHandOverItems()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    DeliverStatus confiremedDeliverStatuses = ctx.DeliverStatuses.Where(x => x.Title == "dispatched_from_store").FirstOrDefault();
                    var data = from O in ctx.Orders //.Where(x => x.DeliveryStatus == confiremedDeliverStatuses.Id && x.UpdatedDate > DateTime.UtcNow.AddDays(-1))
                               join
                                A in ctx.Addreses on O.DeliveryAddressId equals A.Id
                               select new DeliveryReport
                               {
                                   OrderId = O.Id,
                                   WaybillId ="kjlkj", //O.WaybillId,
                                   Name = "test",//string.Format("{0}{1}", (!string.IsNullOrEmpty(A.FirstName) ? A.FirstName : ""), (!string.IsNullOrEmpty(A.LastName) ? A.LastName : "")),
                                   District = A.District,
                                   Address = "test",//string.Format("{0} {1 {2}", (!string.IsNullOrEmpty(A.AddressLine01) ? A.AddressLine01 : ""), (!string.IsNullOrEmpty(A.AddressLine02) ? A.AddressLine02 : ""), (!string.IsNullOrEmpty(A.AddressLine03) ? A.AddressLine03 : "")),
                                   ContactNumber = "test",//string.Format("{0} / {1}", (!string.IsNullOrEmpty(A.ContactNumber1) ? A.ContactNumber1 : ""), (!string.IsNullOrEmpty(A.ContactNumber2) ? A.ContactNumber2 : ""))
                               };
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, string.Format("{0}", ex.Message), "DBHandlerReport", "GetTodayHandOverItems");

                return null;
            }
           
        }

        internal static List<DeliveryReport> GetDeliveryReport(DateTime fromdate ,DateTime to,int DeliveryStatusId,int paymentMethod)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   // DeliverStatus confiremedDeliverStatuses = ctx.DeliverStatuses.Where(x => x.Title == "dispatched_from_store").FirstOrDefault();
                    var data = from O in ctx.Orders .Where(x => 
                                                            (x.DeliveryStatus == DeliveryStatusId)  && 
                                                            (x.UpdatedDate < to) && 
                                                            (x.UpdatedDate > fromdate))
                               join
                                A in ctx.Addreses on O.DeliveryAddressId equals A.Id
                               select new DeliveryReport
                               {
                                   OrderId = O.Id,
                                   WaybillId = (!string.IsNullOrEmpty(O.WaybillId) ? O.WaybillId : ""), 
                                   Name = string.Format("{0}{1}", (!string.IsNullOrEmpty(A.FirstName) ? A.FirstName : ""), (!string.IsNullOrEmpty(A.LastName) ? A.LastName : "")),
                                   District = A.District,
                                   Address = string.Format("{0} {1 {2}", (!string.IsNullOrEmpty(A.AddressLine01) ? A.AddressLine01 : ""), (!string.IsNullOrEmpty(A.AddressLine02) ? A.AddressLine02 : ""), (!string.IsNullOrEmpty(A.AddressLine03) ? A.AddressLine03 : "")),
                                   ContactNumber = string.Format("{0} / {1}", (!string.IsNullOrEmpty(A.ContactNumber1) ? A.ContactNumber1 : ""), (!string.IsNullOrEmpty(A.ContactNumber2) ? A.ContactNumber2 : ""))
                               };
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, string.Format("{0}", ex.Message), "DBHandlerReport", "GetTodayHandOverItems");

                return null;
            }
        }

        internal static List<BookStock> GetBookStocks()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var data = from p in ctx.BookProperties join b in ctx.Books on p.BookId equals b.Id where 
                               !b.isDeleted select new BookStock
                               {
                                   Id = b.Id,
                                   Name=b.Title,
                                   RemainingAmount=p.NumberOfCopies<0?0: p.NumberOfCopies
                               };
                    return data.OrderBy(x=> x.RemainingAmount).ToList();
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, string.Format("{0}", ex.Message), "DBHandlerReport", "GetTodayHandOverItems");

                return null;
            }
        }
    }
}