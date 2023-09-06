using Online_book_shop.Handlers.Business;
using Online_book_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Database
{
    public class DBHandlerPayments
    {
        internal static Cart RevertCartFromKokoPayment(int cartId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var cart = ctx.Carts.Where(x => x.Id == cartId).FirstOrDefault();

                    if (cart != null)
                    {
                        var cartbooks = ctx.Cart_Books.Where(x => x.CartId == cartId).ToList();
                        if (cartbooks != null && cartbooks.Count > 0)
                        {
                            decimal newTotalDiscount = 0;
                            foreach (var book in cartbooks)
                            {
                                var promotion = BusinessHandlerPromotion.Get(book.BookId, book.BookPropertyId);
                                if (promotion != null)
                                {
                                    if (promotion.PromotionMethods == (int)PromotionMethods.Percentage)
                                    {
                                        var percentage = promotion.DiscountValue;
                                        // reduce 10 from percentage because of KOKO
                                        var newpercentage =percentage;
                                        var newdescount = Math.Round((book.AmountBeforeDiscount) * Convert.ToDecimal(newpercentage / 100), 2);
                                        var newAmountAfterDiscount = book.AmountBeforeDiscount - newdescount;
                                        book.AmountAfterDiscount = newAmountAfterDiscount;
                                        book.Discount = newdescount;

                                        newTotalDiscount = newTotalDiscount + newdescount;
                                    }
                                }
                            }
                            cart.Discount = newTotalDiscount;
                            cart.AmountAfterDiscount = cart.AmountBeforeDiscount - newTotalDiscount;

                        }
                        ctx.SaveChanges();
                    }
                    return cart;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static Cart UpdateCartForKokoPayment(int cartId)
        {
          
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var cart = ctx.Carts.Where(x => x.Id == cartId).FirstOrDefault();
                    
                    if (cart != null)
                    {
                        var cartbooks = ctx.Cart_Books.Where(x => x.CartId == cartId).ToList();
                        if (cartbooks != null && cartbooks.Count > 0)
                        {
                            decimal newTotalDiscount = 0;
                            foreach (var book in cartbooks)
                            {
                                var promotion = BusinessHandlerPromotion.Get(book.BookId, book.BookPropertyId);
                                if (promotion != null)
                                {
                                    if (promotion.PromotionMethods == (int)PromotionMethods.Percentage)
                                    {
                                        var percentage = promotion.DiscountValue;
                                        // reduce 10 from percentage because of KOKO
                                        var newpercentage = (percentage - 10);
                                        var newdescount = Math.Round((book.AmountBeforeDiscount) * Convert.ToDecimal(newpercentage / 100), 2);
                                        if (newdescount <0) { 
                                            newdescount = 0;
                                        }
                                        var newAmountAfterDiscount = book.AmountBeforeDiscount - newdescount;
                                        book.AmountAfterDiscount = newAmountAfterDiscount;
                                        book.Discount = newdescount;

                                        newTotalDiscount = newTotalDiscount + newdescount;
                                    }
                                }
                            }
                            cart.Discount = newTotalDiscount;
                            cart.AmountAfterDiscount = cart.AmountBeforeDiscount - newTotalDiscount;

                        }
                        ctx.SaveChanges();
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