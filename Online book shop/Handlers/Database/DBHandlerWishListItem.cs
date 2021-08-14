using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerWishListItem
    {
        public static WishListItem Add(WishListItem model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    WishListItem wishListItem = ctx.WishListItems.Where(x => x.BookId == model.BookId && x.UId == model.UId).FirstOrDefault();
                    if(wishListItem == null)
                    {
                        //add item
                        ctx.WishListItems.Add(model);
                    }
                    else if(wishListItem !=null && wishListItem.isDeleted)
                    {
                        //update
                        wishListItem.isDeleted = false;
                        wishListItem.UpdatedDate = DateTime.UtcNow;
                    }
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerWishListItem", MethodBase.GetCurrentMethod().Name);
                return null;
            }
            return model;
        }

        public static WishListItem Remove(WishListItem model)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    WishListItem wishListItem = ctx.WishListItems.Where(x => x.BookId == model.BookId && x.UId == model.UId).FirstOrDefault();
                    if(wishListItem != null && ! wishListItem.isDeleted)
                    {
                        //update
                        wishListItem.isDeleted = true;
                        wishListItem.UpdatedDate = DateTime.UtcNow;
                    }
                    ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerWishListItem", MethodBase.GetCurrentMethod().Name);
                return null;
            }
            return model;
        }
        public static bool isLovedBook(int bookId, string uid)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    WishListItem wishListItem = ctx.WishListItems.Where(x => x.BookId == bookId && x.UId == uid).FirstOrDefault();
                    if (wishListItem != null && !wishListItem.isDeleted)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                BusinessHandlerMPLog.Log(LogType.Exception, ex.Message, "DBHandlerWishListItem", MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
    }
}