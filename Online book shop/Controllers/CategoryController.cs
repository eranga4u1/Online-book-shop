using Online_book_shop.Handlers.Business;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int id)
        {
            int page = 0;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            ViewBag.Category = BusinessHandlerCategory.GetCategory(id);
            List<BookVMTile> books = BusinessHandlerBook.GetBooksByCategory(id, page);
            ViewBag.TotalNumberOfBooks = books != null ? books.Count : 0;
            ViewBag.Books = books.Skip(12 * page).Take(12).ToList();
            return View();
        }
        public ActionResult Collection()
        {
            return View();
        }
    }
}