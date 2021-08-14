using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerWishListItem
    {
        public static WishListItem Add(WishListItem model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.UpdatedDate = DateTime.UtcNow;
            model.isDeleted = false;
            model.UId = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerWishListItem.Add(model);
        }
        public static WishListItem Remove(WishListItem model)
        {
            model.UpdatedDate = DateTime.UtcNow;
            model.isDeleted = true;
            model.UId = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerWishListItem.Remove(model);
        }
        public static bool isLovedBook(int bookId, string uid)
        {
            return DBHandlerWishListItem.isLovedBook(bookId, uid);
        }
    }
}