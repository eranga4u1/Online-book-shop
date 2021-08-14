using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class Charts
    {
        public int TotalActiveUsers { get; set; }
        public int TotalShippedOrders { get; set; }

        public decimal TotalEarnings { get; set; }
        public int ActiveItemCount { get; set; }

        public Dictionary<string,int> PreviousSixMonthsOrders { get; set; }
        public Dictionary<string, int> PendingOrdersByStatus { get; set; }

        public Dictionary<string, int> Stock { get; set; }
    }
}