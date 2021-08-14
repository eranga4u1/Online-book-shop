using Online_book_shop.Handlers.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserReviewsController : Controller
    {
        // GET: Admin/ManageUserReviews
        public ActionResult Index()
        {
            if(!string.IsNullOrEmpty(Request.QueryString["filter"]) && Request.QueryString["filter"] == "all")
            {
                ViewBag.list = BusinessHandlerUserReviews.GetAll(true);
            }
            else
            {
                ViewBag.list = BusinessHandlerUserReviews.GetAll(false);
            }
            
            return View();
        }
        public ActionResult HideItem(int id)
        {
            BusinessHandlerUserReviews.Hide(id);
            return RedirectToAction("Index");
        }
        public ActionResult ShowItem(int id)
        {
            BusinessHandlerUserReviews.Show(id);
            return RedirectToAction("Index");
        }
    }
}