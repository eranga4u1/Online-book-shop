using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MediaController : Controller
    {
        // GET: Admin/Media
        public ActionResult Index()
        {
            return View();
        }        
    }
}