using Newtonsoft.Json;
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
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            ViewBag.Banners = BusinessHandlerMedia.GetHomeBanners();
            ViewBag.SocialMedeaURLs = BusinessHandlerConfigurations.GetSocialMediaURLs();
            Configuration config = BusinessHandlerConfigurations.GetConfigByKey("CONTACTUS-PAGE");
            if (config != null)
            {
                ViewBag.contactVM = JsonConvert.DeserializeObject<ContactVM>(config.Value);
            }
            else{
                ViewBag.contactVM = new ContactVM { CompanyName = "", AddressLine1 = "", AddressLine2 = "", AddressLine3 = "", Email1 = "", Email2 = "", Email3 = "", Phone1 = "", Phone2 = "", Phone3 = "", Description = "" };
            }
            Configuration configabtUs = BusinessHandlerConfigurations.GetConfigByKey("ABOUT-US");
            if (configabtUs != null)
            {
                ViewBag.AboutUs = new AboutUs { Summary = configabtUs.Value }; 
            }
            else
            {
                ViewBag.AboutUs = new AboutUs { Summary="" };
            }
            return View();
        }
    }
}