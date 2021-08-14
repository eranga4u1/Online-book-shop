using Online_book_shop.Handlers.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class VMDeliveryCharge
    {
        public decimal weight { get; set; }
        public DeliveryTypes delivery_type { get; set; }
        public string area { get; set; }
        public string country { get; set; }
        public decimal cart_amount { get; set; }
        public decimal total_cost { get; set; }
        public decimal total_delivery_amount { get; set; }
    }
}