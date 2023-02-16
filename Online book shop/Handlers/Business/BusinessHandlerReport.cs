using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_book_shop.Models;
using Online_book_shop.Handlers.Database;
using System.Data;
using Online_book_shop.Models.ViewModel.Report;
using Online_book_shop.Models.ViewModel;

namespace Online_book_shop.Handlers.Business
{
    public static class BusinessHandlerReport
    {
        public static List<DeliveryReport> GetTodayHandOverItems()
        {
            return DBHandlerReport.GetTodayHandOverItems();        
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentException("enumerable");
            var dt = new DataTable();
            var es = enumerable as List<T> ?? enumerable.ToList();
            var first = es.First();
            if (first != null)
            {
                var props = first.GetType().GetProperties();
                foreach (var propertyInfo in props)
                {
                    if (!propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.Name.Equals("String"))
                    {
                        dt.Columns.Add(new DataColumn(propertyInfo.Name));
                    }
                }
            }

            foreach (var e in es)
            {
                var props = e.GetType().GetProperties();
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                foreach (var propertyInfo in props)
                {
                    if (!propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.Name.Equals("String"))
                    {
                        dr[propertyInfo.Name] = propertyInfo.GetValue(e);
                    }
                }
            }

            return dt;
        }

        public static List<WebOrder> GetWebOrders(List<Order> orders)
        {
            List<WebOrder> webOrders = new List<WebOrder>();
            foreach(Order order in orders)
            {
                try
                {
                    Address BillingAddress = BusinessHandlerAddress.GetAddress(order.BillingAddressId);
                    Address DeliveryAddress = BusinessHandlerAddress.GetAddress(order.DeliveryAddressId);
                    Cart cart = BusinessHandlerShopingCart.GetById(order.CartId);
                    string paymetMethod = "";
                    string paymentStatus = "";
                    string deliveryMethod = "";
                    if (order.PaymentMethod == 0)
                    {
                        paymetMethod = "Cash On Delivery";
                    }
                    else if (order.PaymentMethod == 1)
                    {
                        paymetMethod = "Online Payment";
                    }
                    else if (order.PaymentMethod == 2)
                    {
                        paymetMethod = "Bank Transfer / Deposit";
                    }
                    else if (order.PaymentMethod == 3)
                    {
                        paymetMethod = "Ez Cash";
                    }
                    else if (order.PaymentMethod == 4)
                    {
                        paymetMethod = "In-Stock Payment";
                    }
                    else if (order.PaymentMethod == 5)
                    {
                        paymetMethod = "BNPL";
                    }

                    if (order.PaymentStatus == 0)
                    {
                        paymentStatus = "Pending Payment";
                    }
                    else if (order.PaymentStatus == 1)
                    {
                        paymentStatus = "Paid";
                    }

                    if (order.DeliveryMethod == 0)
                    {
                        deliveryMethod = "Postal";
                    }
                    else if (order.DeliveryMethod == 1)
                    {
                        deliveryMethod = "Courier";
                    }
                    else if (order.DeliveryMethod == 2)
                    {
                        deliveryMethod = "Collect From Store";
                    }


                    WebOrder webOrder = new WebOrder();
                    webOrder.Date = order.CreatedDate.ToString("dddd, dd MMMM yyyy");
                    webOrder.WayBillId = !string.IsNullOrEmpty(order.WaybillId) ? order.WaybillId : "";
                    webOrder.OrderNumber =  order.Id.ToString();
                    webOrder.InvoiceNumber= !string.IsNullOrEmpty(order.UId) ? order.UId : "";
                    webOrder.ReceiverName = string.Format("{0} {1}",
                        !string.IsNullOrEmpty(DeliveryAddress.FirstName) ? DeliveryAddress.FirstName : "",
                        !string.IsNullOrEmpty(DeliveryAddress.LastName) ? DeliveryAddress.LastName : "");
                    webOrder.DeliveryAddress = string.Format("{0} {1} {2} {3}",
                        !string.IsNullOrEmpty(DeliveryAddress.AddressLine01) ? DeliveryAddress.AddressLine01 : "",
                        !string.IsNullOrEmpty(DeliveryAddress.AddressLine02) ? DeliveryAddress.AddressLine02 : "",
                         !string.IsNullOrEmpty(DeliveryAddress.AddressLine03) ? DeliveryAddress.AddressLine03 : "",
                        !string.IsNullOrEmpty(DeliveryAddress.District) ? DeliveryAddress.District: "");
                    webOrder.DistrictName = !string.IsNullOrEmpty(DeliveryAddress.District) ? DeliveryAddress.District : "";
                    webOrder.ReceiverPhone = string.Format("{0} / {1}",
                        !string.IsNullOrEmpty(DeliveryAddress.ContactNumber1) ? DeliveryAddress.ContactNumber1 : "",
                        !string.IsNullOrEmpty(DeliveryAddress.ContactNumber2) ? DeliveryAddress.ContactNumber2 : "");
                    webOrder.COD = (order.PaymentStatus == (int)PaymentStatus.PendingPayment) ? (cart.AmountAfterDiscount + order.DeliveryCharges).ToString() : "0";
                    webOrder.Description = order.DeliverySpecialNote;
                    webOrder.PayerName = string.Format("{0} {1}",
                        !string.IsNullOrEmpty(BillingAddress.FirstName) ? BillingAddress.FirstName : "",
                        !string.IsNullOrEmpty(BillingAddress.LastName) ? BillingAddress.LastName : "");
                    webOrder.PayerAddress = string.Format("{0} {1} {2} {3}",
                        !string.IsNullOrEmpty(BillingAddress.AddressLine01) ? BillingAddress.AddressLine01 : "",
                        !string.IsNullOrEmpty(BillingAddress.AddressLine02) ? BillingAddress.AddressLine02 : "",
                        !string.IsNullOrEmpty(BillingAddress.AddressLine03) ? BillingAddress.AddressLine03 : "",
                        !string.IsNullOrEmpty(BillingAddress.District) ? BillingAddress.District : "");
                    webOrder.PayerPhone = string.Format("{0} / {1}",
                       !string.IsNullOrEmpty(BillingAddress.ContactNumber1) ? BillingAddress.ContactNumber1 : "",
                       !string.IsNullOrEmpty(BillingAddress.ContactNumber2) ? BillingAddress.ContactNumber2 : "");
                    webOrder.DeliverySpecialNote = order.DeliverySpecialNote;
                    webOrder.PaymentSpecialNote=order.PaymentSpecialNote;
                    webOrder.OrderSummary= order.OrderSummary;
                    webOrder.PaymentMethod = paymetMethod;
                    webOrder.PaymentStatus = paymentStatus;
                    webOrder.DeliveryMethod = deliveryMethod;
                    webOrder.Total = (cart.AmountAfterDiscount + order.DeliveryCharges);
                    webOrder.Email = !string.IsNullOrEmpty(DeliveryAddress.EmailAddress) ? DeliveryAddress.EmailAddress : "";

                    webOrders.Add(webOrder);
                }
                catch(Exception ex)
                {
                    BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "GetWebOrders", "BusinessHandlerRepot");
                }
                
            }
            return webOrders;
        }

        internal static object GetStocks()
        {
            return DBHandlerReport.GetBookStocks();
        }

        public static string GetOrderDescription(int CartId)
        {
            string responce = "";
            try
            {
                Cart _cart = BusinessHandlerShopingCart.GetById(CartId);
                if (_cart != null && _cart.Items != null && _cart.Items.Count > 0)
                {
                    foreach (Cart_Book cb in _cart.Items)
                    {

                        string bookName = "";
                        string isbn = "";
                        string property = "";
                        string numberOfItems = cb.NumberOfItems.ToString();
                        BookVMTile b = BusinessHandlerBook.GetBookTileByBookId(cb.BookId);
                        if(b != null)
                        {
                            bookName = b.BookName;
                            isbn = b.ISBN;
                            foreach (BookProperties p in b.Property)
                            {
                                if (cb.BookPropertyId == p.Id)
                                {
                                    property = p.Title;
                                }
                            }
                            string item = string.Format("{0}({1})-{2}-{3}" + Environment.NewLine, bookName, isbn, property, numberOfItems);
                            responce = responce + item;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "Businesshandlerreport", "get order description");
            }
          
            return responce;
        }
    }
}