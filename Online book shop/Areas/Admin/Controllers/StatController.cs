using Online_book_shop.Handlers.Business;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    public class StatController : Controller
    {
        // GET: Admin/Stat
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaleStat(FilterByDate model=null)
        {
            if(model.fromdate == null && model.todate==null)
            {
                model = new FilterByDate();
                model.fromdate = DateTime.Now.AddMonths(-1);
                model.todate = DateTime.Now;
            }
            ViewBag.Results = BusinessStatHandler.GetBookStats(model);
            ViewBag.fromdate=model.fromdate;
            ViewBag.todate=model.todate;
            return View();
        }
    }
}