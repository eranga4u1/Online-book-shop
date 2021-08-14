using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index(string urlPart)
        {
           Content content= BusinessHandlerContent.GetByUrlPart(urlPart.Trim());
            if(content != null)
            {
                ViewBag.page = content;
                return View();
            }
            else
            {
                return Redirect("/");
            }
          
        }
    }
}