using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class PageResults
    {
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }

        public Object Results { get; set; }
    }
}