using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BookPromotionVM
    {
        public int Id { get; set; }
        public string PromotionTitle { get; set; }
        public string PromotionDescription { get; set; }
        public int PromotionMethods { get; set; }
        public Double DiscountValue { get; set; }
        public int BookPropertyId { get; set; }
        public decimal BookPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal BookPriceAfterDiscount { get; set; }
    }
}