using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerUserReviews
    {
        internal static UserReview Update(UserReview review)
        {
            //UserReview ur = new UserReview();
            //ur.value = value;
            review.UserId = BusinessHandlerAuthor.GetLoginUserId();
            review.CreatedDate = DateTime.UtcNow;
            review.isApproved = false;
            review.CreatedBy= BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerUserReviews.Update(review);

        }
        internal static UserReview UpdateRateOnly(UserReview review)
        {
            //UserReview ur = new UserReview();
            //ur.value = value;
            review.UserId = BusinessHandlerAuthor.GetLoginUserId();
            review.CreatedDate = DateTime.UtcNow;
            review.isApproved = false;
            review.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerUserReviews.UpdateRateOnly(review);

        }

        //internal static UserReview Update(UserReview review)
        //{
        //    //UserReview ur = new UserReview();
        //    //ur.UserComment = comment;
        //    review.UserId = BusinessHandlerAuthor.GetLoginUserId();
        //    review.CreatedDate = DateTime.UtcNow;
        //    review.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
        //    return DBHandlerUserReviews.Update(review);
        //}

        internal static List<ReviewVM> GetReviewsForBook(int Id)
        {
            return DBHandlerUserReviews.GetReviewsForBook(Id);
        }
        internal static int GetRecentReviewForBook(int Id)
        {
            return DBHandlerUserReviews.GetRecentReviewForBook(Id);
        }

        internal static List<UserReview> GetAll(bool withApproved)
        {
            return DBHandlerUserReviews.GetReviews(withApproved);
        }
        internal static bool Show(int id)
        {
            return DBHandlerUserReviews.ShowHide(id, false);
        }
        internal static bool Hide(int id)
        {
            return DBHandlerUserReviews.ShowHide(id, true);
        }
        public static int GetBookRating(int id)
        {
            return DBHandlerUserReviews.GetBookRating(id);
        }

        public static List<BookVMTile> GetWhistListByUser(string uid)
        {
            return DBHandlerUserReviews.GetWhistListByUser(uid);
        }
        public static int GetReviewsAndCommentCount()
        {
            string uid = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerUserReviews.GetReviewsAndCommentCount(uid);
        }
    }
}