using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerShopingCart
    {
        internal static Cart Add(Cart cart)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Carts.Add(cart);
                    if (ctx.SaveChanges() > 0) {
                        return cart;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static bool ChangeCartStatus(CartStatus cartStatus, int cartId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var cart = ctx.Carts.Where(x => x.Id == cartId).FirstOrDefault();
                    if (cart != null)
                    {
                        cart.CartStatus = (int)cartStatus;
                        cart.UpdatedDate = DateTime.UtcNow;
                    }
                    if (ctx.SaveChanges() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool IsPreOrderEnabled(int bookId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    int preOrderId = ctx.SaleStatus.Where(s => s.Title == "pre_order").FirstOrDefault().Id;
                    var book = ctx.Books.Where(x=> x.Id==bookId).FirstOrDefault();
                    if (book != null)
                    {
                       if(book.SaleType== preOrderId)
                        {
                            if (book.RelaseDate < DateTime.UtcNow)
                            {
                                return false;
                            }
                        }
                    }
                   
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static void UpdateCartBookFromOrderId(string orderId, List<Cart_Book> items)
        {
            throw new NotImplementedException();
        }
    }
}