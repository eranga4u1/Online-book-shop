using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerCartBook
    {
        internal static Cart_Book Add(Cart_Book cart_book)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Cart_Books.Add(cart_book);
                    if (ctx.SaveChanges() > 0)
                    {
                        return cart_book;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static List<Cart_Book> Add(List<Cart_Book> cart_books)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Cart_Books.AddRange(cart_books);
                    if (ctx.SaveChanges() > 0)
                    {
                        return cart_books;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Cart GetById(int id)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var cart= ctx.Carts.Where(x => x.Id == id).FirstOrDefault();
                    var cartItems = ctx.Cart_Books.Where(x => x.CartId == id);
                    if(cart != null)
                    {
                        cart.Items = cartItems!=null?cartItems.ToList():null;
                    }
                    return cart;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}