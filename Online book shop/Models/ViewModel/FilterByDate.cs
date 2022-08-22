using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class FilterByDate
    {
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
    }
}