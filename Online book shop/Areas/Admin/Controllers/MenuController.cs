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
    public class MenuController : Controller
    {
       
        // GET: Admin/Menu
        public ActionResult Index()
        {
           Configuration config= BusinessHandlerConfigurations.GetConfigByKey("HOME_TOP_NAVIGATION");
            if(config != null)
            {
                ViewBag.MenuString = config.Value;
            }
            else
            {
                ViewBag.MenuString = "[{ \"href\": \"/\", \"icon\": \"fas fa-home\", \"text\": \"Home\", \"target\": \"_top\", \"title\": \"My Home\" }]";
            }
            return View();
        }
        public ActionResult Footer()
        {
            Configuration config = BusinessHandlerConfigurations.GetConfigByKey("HOME_FOOTER_NAVIGATION");
            if (config != null)
            {
                ViewBag.MenuString = config.Value;
            }
            else
            {
                ViewBag.MenuString = "[{ \"href\": \"/\", \"icon\": \"fas fa-home\", \"text\": \"Home\", \"target\": \"_top\", \"title\": \"My Home\" }]";
            }
            return View();
        }
    }
}