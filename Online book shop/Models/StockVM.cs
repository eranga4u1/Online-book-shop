using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class StockVM
    {
        public int TotalItem { get; set; }
        public int OutOfStock { get; set; }
        public int Available { get; set; }
        public int RedLine { get; set; }


    }
}