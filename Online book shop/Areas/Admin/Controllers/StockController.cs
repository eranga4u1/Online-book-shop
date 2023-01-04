using Online_book_shop.Handlers.Business;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StockController : Controller
    {
        // GET: Admin/Stock
        public ActionResult Index()
        {
            int SelectedAuthorId = 0;
            int SelectedPublisherId = 0;
            int SelectedStockStatusId = 0;
            int PageId = 0;
            int ItemsPerPage = 100000;
            if (!string.IsNullOrEmpty(Request.QueryString["author"]) && Request.QueryString["author"] !="0")
            {
                SelectedAuthorId = int.Parse(Request.QueryString["author"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["publisher"]) && Request.QueryString["publisher"] != "0")
            {
                SelectedPublisherId = int.Parse(Request.QueryString["publisher"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["stockstatus"]) && Request.QueryString["stockstatus"] != "0")
            {
                SelectedStockStatusId = int.Parse(Request.QueryString["stockstatus"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                PageId = int.Parse(Request.QueryString["page"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ItemsPerPage"]))
            {
                ItemsPerPage = int.Parse(Request.QueryString["ItemsPerPage"]);
            }

            List<BookCountVM> filterRslt = BusinessHandlerStock.GetBookStockDetails(SelectedAuthorId,SelectedPublisherId,SelectedStockStatusId,PageId,ItemsPerPage);
            if(filterRslt !=null && !string.IsNullOrEmpty(Request.QueryString["order"]) && Request.QueryString["order"] == "a")
            {
                ViewBag.FilteredBookStock = filterRslt.OrderByDescending(x => x.Count).ToList(); 
            }
            else if (filterRslt != null && !string.IsNullOrEmpty(Request.QueryString["order"]) && Request.QueryString["order"] == "a")
            {
                ViewBag.FilteredBookStock = filterRslt.OrderBy(x => x.Count).ToList(); 
            }
            else {
                ViewBag.FilteredBookStock = filterRslt;
            }
            
            var summary = BusinessHandlerStock.GetStockVM();
            ViewBag.Summary = summary;
            ViewBag.Authers =BusinessHandlerAuthor.GetAuthors();
            ViewBag.Publishers=BusinessHandlerPublisher.GetPublishers();

            return View();
        }

        [HttpPost]
        public bool UpdateBookStock(BookStockVM model)
        {
            if(model != null && model.Amount>0)
            {
                if (!string.IsNullOrEmpty(model.Ids))
                {
                     
                    string[] ids=model.Ids.Split(',');
                    List<Book_Property_Amount> arr = new List<Book_Property_Amount>();
                    foreach (string id in ids)
                    {
                        Book_Property_Amount item = new Book_Property_Amount();
                        item.Amount = Convert.ToInt32(model.Amount);
                        item.BookId = Convert.ToInt32(id.Split('-')[0]);
                        item.BookPropertyId = Convert.ToInt32(id.Split('-')[1]);
                        arr.Add(item);
                    }
                    return BusinessHandlerStock.UpdateStock(arr);
                }
            }
            return false;
        }
    }
}