using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAll(string StartDate, string EndDate,int OrderType, int pageId = 1,int itemPerPage=100,int deliveryStatus = -1,int paymentStatus =-1)
        {
            //List<Order> List= BusinessHandlerOrder.Get(pageId,itemPerPage,deliveryStatus,paymentStatus);
            //int pages= BusinessHandlerOrder.GetPageCount(pageId, itemPerPage, deliveryStatus, paymentStatus);
            List<Order> List = BusinessHandlerOrder.GetFiltered(deliveryStatus, OrderType, StartDate, EndDate);
            int pages = (List.Count / itemPerPage) + 1;
            ViewBag.orders = List;
            ViewBag.pageCount = pages;
            return View();
        }
        public ActionResult GetOrders(int deliveryStatus, int OrderType,string StartDate,string EndDate)
        {
            List<Order> List = BusinessHandlerOrder.GetFiltered(deliveryStatus, OrderType, StartDate, EndDate);
            return View();
        }
        public ActionResult Get(int id)
        {
            Order order= BusinessHandlerOrder.Get(id);
            Cart cart = BusinessHandlerShopingCart.GetById(order.CartId);
            ViewBag.cart = cart;
            return View(order);
        }
        public string ChangeDeliveryStatus(OrderStatus model)
        {
            bool success= BusinessHandlerOrder.ChangeStatus(model.OrderId,model.StatusId,model.TrackingId);
            return success.ToString();
        }

        [HttpPost]
        public JsonResult BulkUpdate(Order[] orders)
        {
            JsonResult jr = new JsonResult();
            Dictionary<string, string> response = new Dictionary<string, string>();
            if (orders !=null && BusinessHandlerOrder.ChangeStatus(orders.ToList()))
            {
                response.Add("state", "success");
            }
            else
            {
                response.Add("state", "failed");
            }
            jr.Data = response;
            return jr;
        }
    }
}