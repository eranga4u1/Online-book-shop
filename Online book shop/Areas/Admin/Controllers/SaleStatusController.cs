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
    public class SaleStatusController : Controller
    {
        // GET: Admin/SaleStatus
        public ActionResult Index()
        {
            ViewBag.SaleStatus = BusinessHandlerSaleStatus.GetAllActiveSaleStatus();
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(SaleStatus model)
        {
            if (BusinessHandlerSaleStatus.Add(model))
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.SaleStatus=BusinessHandlerSaleStatus.GetSaleStatus(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(SaleStatus model)
        {
            if (BusinessHandlerSaleStatus.Update(model))
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult delete(int id)
        {
            ViewBag.Message = "";
            ViewBag.Id = id;
            if (BusinessHandlerSaleStatus.IsAssigned(id))
            {
                ViewBag.Message = "This Type Already Assigned For Some Orders. Please change those and try to delete.";
                return View();
            }
            else
            {
                if (BusinessHandlerSaleStatus.Delete(id))
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