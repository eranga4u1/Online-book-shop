using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel.Report
{
    public class BookStock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RemainingAmount { get; set; }
    }
}