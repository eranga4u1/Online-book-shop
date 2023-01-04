using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class PayHereRequest
    {
        public string merchant_id { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
        public string notify_url { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string order_id { get; set; }
        public string items { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string hash { get; set; }
    }
}