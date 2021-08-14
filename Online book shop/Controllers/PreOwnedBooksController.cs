using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class PreOwnedBooksController : Controller
    {
        // GET: PreOwnedBooks
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult MyBooks()
        {
            return View();
        }
    }
}