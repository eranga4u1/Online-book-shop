using Newtonsoft.Json;
using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Controllers
{
    public class UserReviewsController : Controller
    {
        // GET: UserReviews
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string AddUserRating(UserReview model)
        {   
            return JsonConvert.SerializeObject(BusinessHandlerUserReviews.UpdateRateOnly(model));
        }
        [HttpPost]
        public string AddUserComment(UserReview model)
        {
            return JsonConvert.SerializeObject(BusinessHandlerUserReviews.Update(model));
        }

        public string LoveThisItem(WishListItem model)
        {
            if (BusinessHandlerWishListItem.Add(model) !=null)
            {
                return "done";
            }
            return "failed";
        }
        public string NotLoveThisItem(WishListItem model)
        {
            if (BusinessHandlerWishListItem.Remove(model) != null)
            {
                return "done";
            }
            return "failed";
        }

    }
}