using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin
{
    public class StockManagementController : Controller
    {
        // GET: Admin/StockManagement
        public ActionResult Index()
        {
            return View();
        }
    }
}