using Online_book_shop.Handlers.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_book_shop.Models;

namespace Online_book_shop.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Find(string uid)
        {
            Order order = BusinessHandlerOrder.GetByUID(uid);
            string loginUser = BusinessHandlerAuthor.GetLoginUserId();
            List<string> roles = BusinessHandlerUser.GetLoginUserRoles();
            if (!string.IsNullOrEmpty(loginUser))
            {
                if ((order.CreatedBy == loginUser) || roles.Contains("Admin"))
                {
                    ViewBag.Order = order;
                    Cart cart = BusinessHandlerShopingCart.GetById(order.CartId);
                    ViewBag.cart = cart;
                    return View();
                }
                else
                {
                    return RedirectToAction("UserProfile", "Account");
                }
            }            
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
    }
}