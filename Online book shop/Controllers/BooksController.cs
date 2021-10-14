using Online_book_shop.Handlers.Business;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index()
        {
            int page = 1;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            List<BookVMTile> books = BusinessHandlerBook.GetAllBooksForView(page,40);
            //List<Category> categories = BusinessHandlerCategory.GetCategories();
            ViewBag.TotalNumberOfBooks = books != null ? books.Count : 0;
            ViewBag.Books = books;//.Skip(40 * (page - 1)).Take(40).ToList();
            // ViewBag.Categories = categories;
            return View();
        }
    }
}