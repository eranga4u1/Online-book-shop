using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerShopingCart
    {
        internal static Cart AddToCart(Cart cart,Cart_Book books)
        {
            if (cart.Items == null)
            {
                cart.Items = new List<Cart_Book>();
            }
            if(cart.Items.Where(x=> x.BookPropertyId == books.BookPropertyId).Count() > 0)
            {
                Cart_Book cb = cart.Items.Where(x => x.BookPropertyId == books.BookPropertyId).FirstOrDefault();
                cb.NumberOfItems = cb.NumberOfItems + books.NumberOfItems;
            }
            else
            {
                cart.Items.Add(books);
            }
            
            return cart;
        }
        internal static Cart RemoveFromCart(Cart cart, Cart_Book books)
        {
            var items = cart.Items.Where(x => books.BookPropertyId != x.BookPropertyId);
            if (items != null)
            {
              cart.Items= cart.Items.Where(x=> books.BookPropertyId !=x.BookPropertyId).ToList();
                if (cart.Items.Count < 1)
                {
                    cart.AmountAfterDiscount = 0;
                    cart.AmountBeforeDiscount = 0;
                    cart.Discount = 0;
                }
            }
           

            return cart;
        }
        internal static Cart UpdateCart(Cart cart)
        {
            decimal Amountb4Discount = 0;
            decimal AmountAfterDiscount = 0;
            decimal Discount = 0;
            cart.SessionId = HttpContext.Current.Session.SessionID;
            foreach(var item in cart.Items)
            {
                BookPrice bookPrice = BusinessHandlerBook.GetBookPriceInfo(item.BookId, item.BookPropertyId);
                if(bookPrice != null)
                {
                    if(bookPrice.ItemInStock < 1)
                    {
                        Book book = BusinessHandlerBook.Get(item.BookId);
                        if(book != null)
                        {
                            cart.Messages.Add("Our stock has updated few moment ago, At the moment item : " + book.Title + " has out of stock.Sorry for the inconvienent and this book removed from cart automatically");
                            BusinessHandlerShopingCart.RemoveFromCart(cart, item);
                        }
                        
                    }
                    else if (bookPrice.ItemInStock < item.NumberOfItems)
                    {
                        Book book = BusinessHandlerBook.Get(item.BookId);
                        cart.Messages.Add("Our stock has updated few moment ago, At the moment we only have "+ bookPrice.ItemInStock +" "+ book.Title+ " items in stock.Sorry for the inconvienent and added remaining copies added to cart automatically");
                        item.NumberOfItems = bookPrice.ItemInStock;
                        Amountb4Discount = Amountb4Discount + (bookPrice.UnitPrice * item.NumberOfItems);
                        AmountAfterDiscount = AmountAfterDiscount + (bookPrice.PriceAfterDiscount * item.NumberOfItems);
                        Discount = Discount + (bookPrice.Discount * item.NumberOfItems);

                        item.AmountAfterDiscount = bookPrice.PriceAfterDiscount * item.NumberOfItems;
                        item.AmountBeforeDiscount = bookPrice.UnitPrice * item.NumberOfItems;
                        item.Discount = bookPrice.Discount * item.NumberOfItems;
                    }
                    else
                    {
                        Amountb4Discount = Amountb4Discount + (bookPrice.UnitPrice * item.NumberOfItems);
                        AmountAfterDiscount = AmountAfterDiscount + (bookPrice.PriceAfterDiscount * item.NumberOfItems);
                        Discount = Discount + (bookPrice.Discount * item.NumberOfItems);

                        item.AmountAfterDiscount = bookPrice.PriceAfterDiscount * item.NumberOfItems;
                        item.AmountBeforeDiscount = bookPrice.UnitPrice * item.NumberOfItems;
                        item.Discount = bookPrice.Discount * item.NumberOfItems;
                    }
                }
                cart.AmountBeforeDiscount = Amountb4Discount;
                cart.AmountAfterDiscount = AmountAfterDiscount;
                cart.Discount = Discount;
                cart.UpdatedDate = DateTime.UtcNow;
                cart.CartStatus = (int)CartStatus.DraftCart;
                cart.TotalItemsCount = cart.Items.Sum(x => x.NumberOfItems);
            }
            return cart;
        }

        internal static bool ChangeCartStatus(CartStatus cartStatus, int cartId)
        {
           return DBHandlerShopingCart.ChangeCartStatus(cartStatus,cartId);
        }

        internal static Cart RemoveFromCartAndAddNew(Cart cart, int bookId, int oldbookPropertyId, int newpropertyId)
        {
           Cart_Book cb= cart.Items.Where(x => x.BookPropertyId == oldbookPropertyId && x.BookId == bookId).FirstOrDefault();
            if(cb != null)
            {
                cb.BookPropertyId = newpropertyId;
            }
            return cart;
        }

        internal static Cart CheckoutStage_1(Cart cart)
        {
            cart.CreatedDate = DateTime.UtcNow;
            cart.UpdatedDate = DateTime.UtcNow;
            cart.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            cart.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            if (DBHandlerShopingCart.Add(cart) != null)
            {
                SaleStatus  preOrderType= BusinessHandlerSaleStatus.GetSaleStatusByTitle("pre_order");
                foreach (Cart_Book cb in cart.Items)
                {
                    cb.CartId = cart.Id;
                    cb.CreatedDate = DateTime.UtcNow;
                    cb.UpdatedDate = DateTime.UtcNow;
                    cb.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
                    cb.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
                    Book book = BusinessHandlerBook.Get(cb.BookId);
                    if(preOrderType !=null && book.SaleType== preOrderType.Id)
                    {
                        cb.SpecialNote = "(Preordered book)";
                    }
                }
                DBHandlerCartBook.Add(cart.Items);
            }
            return cart;
        }
        internal static Cart UpdateNumberOfItems(Cart cart, Cart_Book books)
        {
            if (cart.Items == null)
            {
                cart.Items = new List<Cart_Book>();
            }
            if (cart.Items.Where(x => x.BookPropertyId == books.BookPropertyId).Count() > 0)
            {
                Cart_Book cb = cart.Items.Where(x => x.BookPropertyId == books.BookPropertyId).FirstOrDefault();
                cb.NumberOfItems = books.NumberOfItems;
            }
            else
            {
                cart.Items.Add(books);
            }

            return cart;
        }
        internal static Cart GetById(int Id)
        {
            return DBHandlerCartBook.GetById(Id);
        }
        public static decimal GetTotalWeightForCart(Cart cart)
        {
            try
            {
                decimal weightByGram = 0;
                if (cart != null)
                {
                    foreach(Cart_Book cb in cart.Items)
                    {
                        BookProperties bp= BusinessHandlerBookProperties.GetById(cb.BookPropertyId);
                        if(bp != null)
                        {
                            weightByGram = weightByGram + (bp.WeightByGrams * cb.NumberOfItems);
                        }
                    }
                    return weightByGram;
                }
            }
            catch(Exception ex)
            {
                return 0;
            }

            return 0;
        }

        //public static decimal CalculateDeliveryCost(decimal weight, DeliveryTypes type,string Area)
        //{
        //    try
        //    {
        //        BusinessHandlerDeliveryCharges.GetDeliveryCharge(weight,type,Area);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return 0;
        //}
    }
}