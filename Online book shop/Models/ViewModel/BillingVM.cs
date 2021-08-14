using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BillingVM
    {
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public string cardname { get; set; }
        public string cardnumber { get; set; }
        public string expmonth { get; set; }
        public string expyear { get; set; }
        public string cvv { get; set; }

    }
}