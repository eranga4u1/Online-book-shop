using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class ShopingCartController : Controller
    {
        // GET: ShopingCart
        public ActionResult Index()
        {
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
            ViewBag.PostalDeliveryCharges = BusinessHandlerDeliveryCharges.GetPostalCharges();
            ViewBag.CourierCharges = BusinessHandlerDeliveryCharges.GetCourierCharges();

            
            return View();
        }
        [HttpPost]
        public string AddToCart(Cart_Book model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!BusinessHandlerBook.isPreOrderDissabled(model.BookId))
            {
                Cart cart;
                int numberOfItems = model.NumberOfItems;
               
                if (HttpContext.Session["cart"] == null)
                {
                    cart = new Cart();
                    cart.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    cart = Session["cart"] as Cart;
                    Cart_Book cart_Book=  cart.Items.Where(x => x.BookId == model.BookId).FirstOrDefault();
                    numberOfItems = numberOfItems + (cart_Book != null ? cart_Book.NumberOfItems : 0);
                }
                if(BusinessHandlerBook.isOrderLimitExceed(model.BookId, numberOfItems))
                {
                   
                    string message = "Item per order limit exceed !!";
                   
                   
                    result.Add("Message", "Item limit exceed");
                }
                else
                {
                    BusinessHandlerShopingCart.AddToCart(cart, model);
                    BusinessHandlerShopingCart.UpdateCart(cart);
                    Session.Add("cart", cart);
                    result.Add("cart", JsonConvert.SerializeObject(cart));
                    result.Add("minicart", GetUpdatedMiniCart(cart));
                }
              
            }
            else
            {
                result.Add("Message", "Pre Order Expired");
            }
            
            return JsonConvert.SerializeObject(result);
        }
        [HttpPost]
        public string RemoveFromCart(Cart_Book model)
        {
            Cart cart;
            if (Session["cart"] != null)
            {
                cart = Session["cart"] as Cart;
                BusinessHandlerShopingCart.RemoveFromCart(cart, model);
                BusinessHandlerShopingCart.UpdateCart(cart);
                Session["cart"] = cart;
                return JsonConvert.SerializeObject(cart);
                // return GetUpdatedMiniCart();//JsonConvert.SerializeObject(cart);
            }
            else
            {
                return null;
            }
        }

        public string ChangeBookProperty(ChangeBookPropertyVM model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Cart cart;
            if (Session["cart"] != null)
            {
                cart = Session["cart"] as Cart;
                BusinessHandlerShopingCart.RemoveFromCartAndAddNew(cart, model.bookId, model.OldbookPropertyId, model.NewpropertyId);
                BusinessHandlerShopingCart.UpdateCart(cart);
                Session["cart"] = cart;
                result.Add("message", "success");
                return JsonConvert.SerializeObject(result);
            }

            result.Add("message", "failed");
            return JsonConvert.SerializeObject(result);
        }
        public string ChangeAmountOfItems(Cart_Book model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Cart cart;
            int numberOfItems = model.NumberOfItems;
            if (Session["cart"] != null)
            {
                cart = Session["cart"] as Cart;
                //Cart_Book cart_Book = cart.Items.Where(x => x.BookId == model.BookId).FirstOrDefault();
               // numberOfItems = numberOfItems + (cart_Book != null ? cart_Book.NumberOfItems : 0);
                if (BusinessHandlerBook.isOrderLimitExceed(model.BookId, numberOfItems))
                {
                    Book book = BusinessHandlerBook.Get(model.BookId);
                    string message = "Item per order limit exceed !!";
                    if (book != null)
                    {
                        message = string.Format("Item per order limit exceed for {0} !!", book.Title);
                    }

                    cart.ClientMessage = message;
                }
                else
                {
                    cart.ClientMessage = null;
                    cart = BusinessHandlerShopingCart.UpdateNumberOfItems(cart, model);
                    cart = BusinessHandlerShopingCart.UpdateCart(cart);
                    Session["cart"] = cart;
                    result.Add("message", "success");
                }
               
                return JsonConvert.SerializeObject(result);
            }

            result.Add("message", "failed");
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string AddVoucherCode(string code)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Voucher v = BusinessHandlerVoucher.GetActiveVoucherByCode(code);
            if(v != null)
            {
                Cart cart;
                if (Session["cart"] != null)
                {
                    cart = Session["cart"] as Cart;
                    cart.VoucherCode = code;
                    Session["cart"] = cart;
                    result.Add("Ok", "True");
                    result.Add("message", "You entered Rs." + v.VoucherAmount + " Voucher.");
                    result.Add("voucher", JsonConvert.SerializeObject(v));
                    return JsonConvert.SerializeObject(result); 
                }
            }
            result.Add("Ok", "False");
            result.Add("message", "Voucher validation failed");
            return JsonConvert.SerializeObject(result); ;
        }
        
        public string GetUpdatedMiniCart(Cart cart)
        {
         string htmlContent= "<div class=\"minicart-padding\">"+
                                "<div class=\"cartheader\">"+
                                    "<ul>"+
                                        "<li class=\"viewcart\"><a href = \"/ShopingCart\" > View Cart</a></li>"+
                                        "<li class=\"checkout\"><a href = \"/Delivery\" > Checkout </a ></li>"+
                                    "</ul>"+
                                "</div>";

                                if(Session["cart"] != null)
                                {
                                    //Cart cart = Session["cart"] as Cart;
                                    List<Cart_Book> cartItems = cart.Items;
                                    int numberOfItems=cartItems.Sum(x => x.NumberOfItems);
                                    foreach (Cart_Book cb in cartItems)
                                    {
                                        BookVMTile b = BusinessHandlerBook.GetSearchedBookForView(cb.BookId);
                                        string subHtmlContent = "<div class=\"books-listing\">" +
                                                                "<div class=\"item\">" +
                                                                    "<div class=\"bookname\">" +
                                                                        b.BookName +
                                                                    "</div>" +
                                                                    "<div class=\"quantity\">" +
                                                                        "x "+cb.NumberOfItems +
                                                                    "</div>" +
                                                                    "<div class=\"price\">" +
                                                                        "Rs. "+cb.AmountAfterDiscount +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>";
                                        htmlContent = htmlContent + subHtmlContent;
                                    }
                                htmlContent= htmlContent+"<div class=\"summary\">"+
                                                        "<div class=\"row-1\">" +
                                                            "<div class=\"total-item\">" +
                                                               " Total Item(s)" +
                                                            "</div>" +
                                                            "<div class=\"total-count\">" +
                                                               numberOfItems +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<div class=\"row-2\">" +
                                                            "<div class=\"total\">" +
                                                                "Total" +
                                                            "</div>" +
                                                            "<div class=\"total-price\">" +
                                                                "Rs. "+cart.AmountAfterDiscount+
                                                            "</div>" +
                                                        "</div>" +
                                                        "<div class=\"row-2\">" +
                                                            "<div class=\"total\">" +
                                                                "You are saving" +
                                                            "</div>" +
                                                            "<div class=\"total-price\">" +
                                                                "Rs. " + cart.Discount +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>";
                                }
                                htmlContent = htmlContent + "</div>";
            return htmlContent;
        }
    }
}