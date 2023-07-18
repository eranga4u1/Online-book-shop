using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Invoice;
using Online_book_shop.Handlers.Notifications;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using Online_book_shop.Payments.MintPaySDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult PaymentResponse()
        {
            BusinessHandlerMPLog.Log(LogType.Message, string.Format("Returned From Payhere ({0})", Request.Url.AbsoluteUri), "Payment", "PaymentResponse");
            string orderUid = Request.QueryString["order_id"];
            string state= Request.QueryString["state"];
            DeliverStatus tempDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("temp_cart"); 
            DeliverStatus partiallyConfirmedDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("partially_confirmed");
            ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
            INotification PrintedNotification = new EmailNotifications();
            if (state.ToLower().Trim() == "done")
            {
                if (orderUid != null)
                {
                    Order order = BusinessHandlerDelivery.Get(orderUid);
                    if (order != null && (order.DeliveryStatus== tempDeliverStatuses.Id || order.DeliveryStatus == partiallyConfirmedDeliverStatuses.Id))
                    {
                        Cart cart = BusinessHandlerShopingCart.GetById(order != null ? order.CartId : 0);
                        // BusinessHandlerDelivery.ChangeOrderStatus();
                        DeliverStatus confiremedDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("confirmed_order");

                        bool gate_1 = BusinessHandlerDelivery.ChangeDeliveryStatus(confiremedDeliverStatuses, order.Id);
                        bool gate_2 = BusinessHandlerDelivery.ChangePaymentStatus((order.PaymentMethod == (int)PaymentMethods.Cash_On_Delivery) ? PaymentStatus.PendingPayment : PaymentStatus.Paid, order.Id);
                        bool gate_3 = BusinessHandlerShopingCart.ChangeCartStatus(CartStatus.OrderConfirmedCart, order.CartId);
                        bool gate_4 = BusinessHandlerStockEntry.Update(cart.Items, ("Order Id" + order.Id.ToString()));
                        //PDFHandler.GenaratePDF(order, cart, "\\Content\\UploadFiles\\Invoices", "Invoice_" + order.UId + "_" + order.CartId + ".pdf",true);
                        if (gate_1 && gate_2 && gate_3 && gate_4)
                        {
                            //Send recept to email;
                            if (Session["cart"] != null)
                            {
                                Session["cart"] = null;
                            }

                            PrintedNotification.EmailInvoice(user, order, cart);
                            ViewBag.Message = "Success";
                        }
                        else
                        {
                            PrintedNotification.EmailInvoice(user, order, cart);
                            BusinessHandlerMPLog.Log(LogType.Exception,"Failed Gate Check", "Payment", "PaymentResponse",string.Format("{0}-{1}-{2}-{3}",gate_1.ToString(),gate_2.ToString(),gate_3.ToString(),gate_4.ToString()));
                            ViewBag.Message = "Failed";
                        }
                        ViewBag.Order = order;

                    }
                    else
                    {
                        BusinessHandlerMPLog.Log(LogType.Message,"Order null or already confirmed Order", "Payment", "PaymentResponse");
                    }
                }
                else
                {
                    BusinessHandlerMPLog.Log(LogType.Exception, "Returned Order UID null From Payhere", "Payment", "PaymentResponse");
                }
            }
            else
            {
                BusinessHandlerMPLog.Log(LogType.Exception, "Returned Failed From Payhere", "Payment", "PaymentResponse");

                ViewBag.Message = "Failed";
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult PaymentRequest(PayHereRequest model)
        {
            BusinessHandlerPayment b = new BusinessHandlerPayment();
            b.PaymentRequestAsync(model);
            //BusinessHandlerPayment.PaymentRequestAsync(model).Wait();
            return Redirect("");
        }
        [HttpPost]
        public ActionResult KokoPaymentRequest(KokoRequest model)
        {
            BusinessHandlerPayment b = new BusinessHandlerPayment();
            var rsult= b.KokoRequest(model);
            return Redirect(rsult);
        }
        [HttpPost]
        public ActionResult MintPayPaymentRequest(ProcessOrder model)
        {
            
           MintPayResponse response = BusinessHandlerPayment.MintPayPayment(model);
            //BusinessHandlerPayment.PaymentRequestAsync(model).Wait();
             ViewBag.purchaseId = response.data;
            return View("MintPay");
           // return Redirect(response.data);
        }
    }
}