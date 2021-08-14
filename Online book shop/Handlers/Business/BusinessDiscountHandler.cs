using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessDiscountHandler
    {
        //To be impliment

            //Single book discount
        internal static decimal GetDiscountAmountForBook(int bookId, int bookPropId,decimal unitPrice)
        {
            BookPromotionVM model = BusinessHandlerPromotion.GetPromotionForBook(bookId,bookPropId,unitPrice);
            return model.DiscountAmount;
        }
        //Cart discount
        //Category discount
    }
}