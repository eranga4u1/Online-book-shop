using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class PublisherController : Controller
    {
        // GET: Publisher
        public ActionResult Index(int id)
        {
            int page = 1;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            ViewBag.publisher = BusinessHandlerPublisher.GetPublisherById(id);
            List<BookVMTile> books = BusinessHandlerBook.GetBooksByPublisher(id, page);
            ViewBag.TotalNumberOfBooks = books != null ? books.Count : 0;
            ViewBag.Books = books.OrderByDescending(x=> x.Id).Skip(12 * (page-1)).Take(12).ToList();
            if (id == 9)
            {
               ViewBag.MethodName= "muses";
            }
            return View();
        }
        public ActionResult Collection()
        {
            int page = 0;
            int itemPerPage = 12;
            if (Request.QueryString["page"] != null)
            {
                page = Convert.ToInt32(Request.QueryString["page"]);
            }
            List<Publisher> publishers = BusinessHandlerPublisher.GetPublishers();
            if (publishers != null)
            {
                ViewBag.publishers = publishers.Skip(page * itemPerPage).Take(itemPerPage).ToList();

            }
            else
            {
                ViewBag.publishers = null;
            }
            ViewBag.TotalNumberOfItems = publishers != null ? publishers.Count : 0;
            return View();
        }
    }
}