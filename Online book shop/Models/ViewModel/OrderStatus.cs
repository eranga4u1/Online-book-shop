using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class OrderStatus
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public string Note { get; set; }
        public string TrackingId { get; set; }
    }
}