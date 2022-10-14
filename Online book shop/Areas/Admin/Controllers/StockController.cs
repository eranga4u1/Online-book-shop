using Online_book_shop.Handlers.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    public class StockController : Controller
    {
        // GET: Admin/Stock
        public ActionResult Index()
        {
            int SelectedAuthorId = 0;
            int SelectedPublisherId = 0;
            int SelectedStockStatusId = 0;
            int PageId = 0;
            int ItemsPerPage = 10;
            if (!string.IsNullOrEmpty(Request.QueryString["author"]))
            {
                SelectedAuthorId = int.Parse(Request.QueryString["author"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["publisher"]))
            {
                SelectedPublisherId = int.Parse(Request.QueryString["publisher"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["stockstatus"]))
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
            ViewBag.FilteredBookStock = BusinessHandlerStock.GetBookStockDetails(SelectedAuthorId,SelectedPublisherId,SelectedStockStatusId,PageId,ItemsPerPage);
            var summary = BusinessHandlerStock.GetStockVM();
            ViewBag.Summary = summary;
            ViewBag.Authers =BusinessHandlerAuthor.GetAuthors();
            ViewBag.Publishers=BusinessHandlerPublisher.GetPublishers();

            return View();
        }
    }
}