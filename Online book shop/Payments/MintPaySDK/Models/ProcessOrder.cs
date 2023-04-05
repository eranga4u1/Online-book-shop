using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Payments.MintPaySDK.Models
{
    public class ProcessOrder
    {
        public string OrderId { get; set; }
        public int CartId { get; set; }

        public decimal TotalAmount { get; set; }
    }
}