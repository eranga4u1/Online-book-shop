using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class VMBulkPromotion
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Percentage { get; set; }
        public string SelectedItems { get; set; }
    }
}