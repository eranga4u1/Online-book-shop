using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Handlers.Invoice;
using Online_book_shop.Handlers.Notifications;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Online_book_shop.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Delivery
        //[Authorize(Roles = "User")]
        public ActionResult Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = "/Delivery" }));
            }
            ViewBag.PostalDeliveryCharges = BusinessHandlerDeliveryCharges.GetPostalCharges();
            ViewBag.CourierCharges = BusinessHandlerDeliveryCharges.GetCourierCharges();
            ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
            ViewBag.User = user;
            Cart cart;
            if (HttpContext.Session["cart"] == null)
            {
                cart = new Cart();
                cart.CreatedDate = DateTime.UtcNow;
            }
            else
            {
                cart = Session["cart"] as Cart;
            }
            ViewBag.Cart = cart;
            ViewBag.Addreses = BusinessHandlerAddress.GetAddresses();
            List<PaymentMethod> Obj_payment_option = new List<PaymentMethod>();
            Configuration conf_payment_option = BusinessHandlerConfigurations.GetConfigByKey("PAYMENT_METHODS");
            if (conf_payment_option != null && !string.IsNullOrEmpty(conf_payment_option.Value))
            {
                Obj_payment_option = JsonConvert.DeserializeObject<List<PaymentMethod>>(conf_payment_option.Value);
            }
            ViewBag.PaymenOption = Obj_payment_option;
            Configuration conf_district = BusinessHandlerConfigurations.GetConfigByKey("DISTRICT_SRILANKA");
            Configuration conf_countries = BusinessHandlerConfigurations.GetConfigByKey("COUNTRIES");
            ViewBag.District = JsonConvert.DeserializeObject<List<string>>(conf_district.Value);
            ViewBag.Countries = JsonConvert.DeserializeObject<List<string>>(conf_countries.Value);
            return View();
        }
        public ActionResult PlaceAnOrder(Order order)
        {
            if(order != null)
            {
                Address billingAddress = BusinessHandlerAddress.GetAddress(order.BillingAddressId);
                Address deliveryAddress = BusinessHandlerAddress.GetAddress(order.DeliveryAddressId);
                if(billingAddress==null || deliveryAddress == null)
                {
                    return RedirectToAction("Index",new {error="Please add valid address"});
                }
                order.FirstName = deliveryAddress.FirstName;
                order.LastName = deliveryAddress.LastName;
                order.DeliveryAddress = (!string.IsNullOrEmpty(deliveryAddress.AddressLine01) ? (deliveryAddress.AddressLine01 + ", ") : "") +
                                        (!string.IsNullOrEmpty(deliveryAddress.AddressLine02) ? (deliveryAddress.AddressLine02 + ", ") : "") +
                                        (!string.IsNullOrEmpty(deliveryAddress.AddressLine03) ? (deliveryAddress.AddressLine03 + ", ") : "") +
                                        (!string.IsNullOrEmpty(deliveryAddress.City) ? (deliveryAddress.City + ", ") : "") +
                                         (!string.IsNullOrEmpty(deliveryAddress.Country) ? (deliveryAddress.Country + ", ") : "") +
                                         (!string.IsNullOrEmpty(deliveryAddress.PostalCode) ? (deliveryAddress.PostalCode + ", ") : "");

                order.BillingAddress = (!string.IsNullOrEmpty(billingAddress.FirstName) ? (billingAddress.FirstName + " ") : "") +
                                        (!string.IsNullOrEmpty(billingAddress.LastName) ? (billingAddress.LastName + ", ") : "") +
                                        (!string.IsNullOrEmpty(billingAddress.AddressLine01) ? (billingAddress.AddressLine01 + ", ") : "") +
                                        (!string.IsNullOrEmpty(billingAddress.AddressLine02) ? (billingAddress.AddressLine02 + ", ") : "") +
                                        (!string.IsNullOrEmpty(billingAddress.AddressLine03) ? (billingAddress.AddressLine03 + ", ") : "") +
                                        (!string.IsNullOrEmpty(billingAddress.City) ? (billingAddress.City + ", ") : "") +
                                         (!string.IsNullOrEmpty(billingAddress.Country) ? (billingAddress.Country + ", ") : "") +
                                         (!string.IsNullOrEmpty(billingAddress.PostalCode) ? (billingAddress.PostalCode + ", ") : "");

                if (HttpContext.Session["cart"] != null)
                {
                    Cart cart = Session["cart"] as Cart;
                    cart.SelectedDeliveryAddress = order.DeliveryAddressId;
                    cart.SelectedBillingAddress = order.BillingAddressId;
                    cart.SelectedDeliveryMethod = order.DeliveryMethod;
                    cart.SelectedPaymentMethod = order.PaymentMethod;
                    cart.AddedPaymentSpecialNote = order.PaymentSpecialNote;
                    // cart.AddedDeliverySpecialNote = BusinessHandlerReport.GetOrderDescription(cart.Id); //order.DeliverySpecialNote; ;
                }
                //if (order.update_ac_info == 1)
                //{
                //    ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
                //    user.FirstName = order.FirstName;
                //    user.LastName = order.LastName;
                //    user.Address = order.DeliveryAddress;
                //    user.ContactNumber = order.ContactNumber;
                //    user.Email = order.EmailAddress;
                //    BusinessHandlerUser.Update(user);
                //}
                if (Session["cart"] != null)
                {
                    Cart cart = Session["cart"] as Cart;
                    cart.CartStatus = (int)CartStatus.DraftCart;
                    cart = BusinessHandlerShopingCart.CheckoutStage_1(cart);
                    order.CartId = cart.Id;
                    decimal orderWeight = BusinessHandlerShopingCart.GetTotalWeightForCart(cart);
                    order.DeliveryCharges = (((DeliveryTypes)order.DeliveryMethod == DeliveryTypes.In_Store_Pickup) ? 0 : BusinessHandlerDeliveryCharges.GetDeliveryCharge(orderWeight, (DeliveryTypes)order.DeliveryMethod, deliveryAddress.District, deliveryAddress.Country));


                    Order _order = BusinessHandlerDelivery.Post(order);
                    ViewBag.Order = _order;

                    cart.OrderId = _order.UId;
                    Session["cart"] = cart;

                    ViewBag.Cart = cart;
                    ViewBag.MerchantId = System.Configuration.ConfigurationManager.AppSettings["MerchantId"];
                    ViewBag.PayHereUrl = System.Configuration.ConfigurationManager.AppSettings["PayHereUrl"];
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                if (order.PaymentMethod == (int)PaymentMethods.Cash_On_Delivery)
                {
                    return RedirectToAction("CashOnDelivery", new RouteValueDictionary(new { controller = "Delivery", action = "CashOnDelivery", Ref = order.UId }));
                }
                else if (order.PaymentMethod == (int)PaymentMethods.Bank_Deposit)
                {
                    return RedirectToAction("CashOnDelivery", new RouteValueDictionary(new { controller = "Delivery", action = "BankDeposit", Ref = order.UId }));
                }
                else if (order.PaymentMethod == (int)PaymentMethods.In_store_payment)
                {
                    return RedirectToAction("InStorePickup", new RouteValueDictionary(new { controller = "Delivery", action = "InStorePickup", Ref = order.UId }));
                }
                else
                {
                    ViewBag.Order = order;
                    //ViewBag.SelectedCity =
                    ViewBag.Cart = BusinessHandlerShopingCart.GetById(order != null ? order.CartId : 0);
                    return View();
                }
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }                       
        }
        public ActionResult BankDeposit(string Ref)
        {
            Order order = BusinessHandlerDelivery.Get(Ref);
            ViewBag.Order = order;
            ViewBag.Cart = BusinessHandlerShopingCart.GetById(order != null ? order.CartId : 0);
            return View();
        }
        public ActionResult InStorePickup(string Ref)
        {
            Order order = BusinessHandlerDelivery.Get(Ref);
            ViewBag.Order = order;
            ViewBag.Cart = BusinessHandlerShopingCart.GetById(order != null ? order.CartId : 0);
            return View();
        }
        public ActionResult CashOnDelivery(string Ref)
        {
            Order order= BusinessHandlerDelivery.Get(Ref);
            ViewBag.Order = order;
            ViewBag.Cart = BusinessHandlerShopingCart.GetById(order !=null?order.CartId:0);
            return View();
        }
        public ActionResult Payment()
        {

            return View();
        }
        public ActionResult OrderConfirmation()
        {
            string orderUid = Request.QueryString["Ref"];
            DeliverStatus tempDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("temp_cart");
            DeliverStatus partiallyConfirmedDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("partially_confirmed");

            // mplog
            if (orderUid != null)
            {
                Order order = BusinessHandlerDelivery.Get(orderUid);
                if (order != null && (order.DeliveryStatus == tempDeliverStatuses.Id || order.DeliveryStatus == partiallyConfirmedDeliverStatuses.Id))
                {
                    if (order != null)
                    {
                        Cart cart = BusinessHandlerShopingCart.GetById(order != null ? order.CartId : 0);
                        // BusinessHandlerDelivery.ChangeOrderStatus();
                        if (!string.IsNullOrEmpty(cart.VoucherCode))
                        {
                            BusinessHandlerVoucher.Spendvoucher(cart.VoucherCode, order.Id);
                        }
                        DeliverStatus confiremedDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("confirmed_order");
                        bool gate_1 = BusinessHandlerDelivery.ChangeDeliveryStatus(confiremedDeliverStatuses, order.Id);
                        bool gate_2 = BusinessHandlerDelivery.ChangePaymentStatus(((order.PaymentMethod == (int)PaymentMethods.Cash_On_Delivery) || (order.PaymentMethod == (int)PaymentMethods.Bank_Deposit)) ? PaymentStatus.PendingPayment : PaymentStatus.Paid, order.Id);
                        bool gate_3 = BusinessHandlerShopingCart.ChangeCartStatus(CartStatus.OrderConfirmedCart, order.CartId);
                        bool gate_4 = BusinessHandlerStockEntry.Update(cart.Items, ("Order Id" + order.Id.ToString()));

                        //  PDFHandler.GenaratePDF(order,cart, "\\Content\\UploadFiles\\Invoices","Invoice_"+order.UId+"_"+order.CartId+".pdf");
                        if (gate_1 && gate_2 && gate_3 && gate_4)
                        {
                            //Send recept to email;
                            if (Session["cart"] != null)
                            {
                                Session["cart"] = null;
                            }

                            ViewBag.Message = "Success";
                            INotification PrintedNotification = new EmailNotifications();
                            ApplicationUser user = BusinessHandlerUser.GetApplicationUser();
                            if (user != null)
                            {
                                PrintedNotification.EmailInvoice(user, order, cart);
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Failed";
                        }

                        ViewBag.Order = order;
                    }
                }
                else if(order != null)
                {
                    ViewBag.Order = order;
                    ViewBag.Message = "Success";
                }
                else
                {
                    ViewBag.Order = null;
                    ViewBag.Message = string.Format( "Issue Occured please contact admin with following {0}", orderUid);
                }
                    
                
            }
            
            return View();
        }
        public ActionResult PaymentResponse()
        {
            return View();
        }
        [Authorize]
        public string UpdateOrderDescription()
        {
            //JsonResult jr = new JsonResult();
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (BusinessHandlerScheduller.UpdateOrderDescription())
            {
                return "True";
                //result.Add("status","true");
            }
            else
            {
                return "false";
               // result.Add("status", "false");
            }
            //jr.Data = result;
            
        }
        [HttpPost]
        public JsonResult GetEstimateCost(VMDeliveryCharge model)
        {
            JsonResult jr = new JsonResult();
            model.total_delivery_amount = BusinessHandlerDeliveryCharges.GetDeliveryCharge(model.weight, model.delivery_type, model.area,model.country);
            model.total_cost = model.total_delivery_amount + model.cart_amount;
            jr.Data = model;
            return jr;
        }

        [HttpPost]

        public string PartiallyConfirmedOrder(OrderStatus model)
        {
            DeliverStatus confiremedDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("partially_confirmed");
            bool success = BusinessHandlerOrder.ChangeStatus(model.OrderId, confiremedDeliverStatuses.Id,null);
            return success.ToString();
        }
        public string ChangeDeliveryStatus(OrderStatus model)
        {
            DeliverStatus confiremedDeliverStatuses = BusinessHandlerDeliveryStatus.GetDeliverStatusByTitle("confirmed_received");
            if(confiremedDeliverStatuses != null)
            {
                bool success = BusinessHandlerOrder.ChangeStatus(model.OrderId, confiremedDeliverStatuses.Id, model.TrackingId);
                return success.ToString();
            }          
            return false.ToString();
           
        }


    }
}