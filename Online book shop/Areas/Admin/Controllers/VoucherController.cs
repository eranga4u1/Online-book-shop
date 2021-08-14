
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
    public class VoucherController : Controller
    {
        // GET: Admin/Voucher
        public ActionResult Index()
        {
            ViewBag.ActiveVouchers = BusinessHandlerVoucher.GetAllVouchers();
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Voucher model)
        {
            Voucher v= BusinessHandlerVoucher.GenarateVoucherCode(model.VoucherAmount, model.NumberOfValidCodes);
            BusinessHandlerVoucher.AddVoucher(v);
            return RedirectToAction("Index");
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerVoucher.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerVoucher.Show(id);
            return RedirectToAction("Index");
        }
    }
}