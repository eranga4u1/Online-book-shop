using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel.Report
{
    public class DeliveryReport
    {
        public int OrderId { get; set; }
        public string WaybillId { get; set; }
        public string Description { get; set; } = "Books";
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }

        public string District { get; set; }
    }
}