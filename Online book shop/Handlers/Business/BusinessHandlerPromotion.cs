using Newtonsoft.Json;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerPromotion
    {
        public static List<Promotion> GetPromotionForBook(int bookId)
        {
            var promotion = DBHandlerPromotion.Get(bookId);
            return promotion;
        }

        internal static List<Promotion> Get()
        {
            return DBHandlerPromotion.Get();
        }
        public static Promotion Get(int bookId, int PropertyId)
        {
            return DBHandlerPromotion.Get(bookId, PropertyId);
        }
        public static BookPromotionVM GetPromotionForBook(int bookId,int propertyId, decimal price)
        {
            BookPromotionVM model = new BookPromotionVM();
            BookPromotionVM BookPromotion = null;
            var promotion = GetPromotionForBook(bookId).Select(x => new BookPromotionVM {
                Id = x.Id,
                PromotionTitle = x.PromotionTitle,
                PromotionDescription = x.PromotionDescription,
                DiscountValue = x.DiscountValue,
                PromotionMethods=x.PromotionMethods,
                BookPropertyId = !string.IsNullOrEmpty(x.OtherParameters)?JsonConvert.DeserializeObject<PromptionParameters>(x.OtherParameters).BookPropertyId:0
            });
            if(promotion != null && promotion.Count()>0)
            {
                BookPromotion= promotion.Where(x => x.BookPropertyId == propertyId).FirstOrDefault();
                //if (BookPromotion == null)
                //{
                //    BookPromotion = promotion.FirstOrDefault();
                //}
            }
            if(BookPromotion != null)
            {
                BookPromotion.BookPrice = price;
                if (BookPromotion.PromotionMethods == (int)PromotionMethods.DeductFromFinalAmount)
                {
                    BookPromotion.DiscountAmount = 0;
                    BookPromotion.BookPriceAfterDiscount = price;
                }
                else if (BookPromotion.PromotionMethods == (int) PromotionMethods.FixedAmount)
                {
                    BookPromotion.DiscountAmount = Convert.ToDecimal(BookPromotion.DiscountValue);
                    BookPromotion.BookPriceAfterDiscount= price-Convert.ToDecimal(BookPromotion.DiscountValue);
                }
                else if (BookPromotion.PromotionMethods == (int) PromotionMethods.Percentage)
                {
                    BookPromotion.DiscountAmount = Math.Round((price * Convert.ToDecimal(BookPromotion.DiscountValue) / 100), 2); ;
                    BookPromotion.BookPriceAfterDiscount = price - BookPromotion.DiscountAmount;
                }
                else
                {
                    BookPromotion.DiscountAmount = 0;
                    BookPromotion.BookPriceAfterDiscount = price;
                }
            }
            else
            {
                BookPromotion = new BookPromotionVM
                {
                    Id = 0,
                    PromotionTitle = "",
                    PromotionDescription = "",
                    DiscountValue = 0,
                    PromotionMethods = -1,
                    BookPropertyId = propertyId,
                    DiscountAmount = 0,
                    BookPriceAfterDiscount = price
                };

            }
            return BookPromotion;
        }

        internal static Promotion Update(Promotion promotion)
        {
            promotion.UpdatedDate = DateTime.UtcNow;
            promotion.isDeleted = false;
            promotion.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerPromotion.Update(promotion);
        }

        internal static  bool Show(int id)
        {
            return DBHandlerPromotion.ShowHide(id, false);
        }

        internal static bool Hide(int id)
        {
            return DBHandlerPromotion.ShowHide(id, true);
        }

        public static Promotion Add(Promotion promotion)
        {
            promotion.CreatedDate = DateTime.UtcNow;
            promotion.UpdatedDate = DateTime.UtcNow;
            promotion.isDeleted = false;
            promotion.CreatedBy = BusinessHandlerAuthor.GetLoginUserId();
            promotion.UpdatedBy = BusinessHandlerAuthor.GetLoginUserId();
            return DBHandlerPromotion.Add(promotion);
        }

        public static Promotion Get(int id)
        {
            return DBHandlerPromotion.GetById(id);
        }
        public static List<Promotion> GetLatestPromotion()
        {
            return DBHandlerPromotion.GetLatestPromotion();
        }

        internal static bool AddNewPromotions(List<Promotion> promoList, int v)
        {
            return DBHandlerPromotion.AddNewPromotions(promoList,v);
        }
    }
}