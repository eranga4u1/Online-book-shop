using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeliverStatusController : Controller
    {
        // GET: Admin/DeliverStatus
        public ActionResult Index()
        {
            ViewBag.DeliverStatus = BusinessHandlerDeliveryStatus.GetAllActiveDeliverStatus();
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(DeliverStatus model)
        {
            if (BusinessHandlerDeliveryStatus.Add(model))
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.DeliverStatus = BusinessHandlerDeliveryStatus.GetDeliverStatus(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(DeliverStatus model)
        {
            if (BusinessHandlerDeliveryStatus.Update(model))
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            ViewBag.Message = "";
            ViewBag.Id = id;
            if (BusinessHandlerDeliveryStatus.IsAssigned(id))
            {
                ViewBag.Message = "This Type Already Assigned For Some Orders. Please change those and try to delete.";
                return View();
            }
            else
            {
                if (BusinessHandlerDeliveryStatus.Delete(id))
                {
                    ViewBag.Message = "Deleted Successfully.";
                }
                else
                {
                    ViewBag.Message = "Failed";
                }
               
                return View();
            } 
        }
    }
}