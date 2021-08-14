using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BookPrice
    {
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public int ItemInStock { get; set; }
    }
}