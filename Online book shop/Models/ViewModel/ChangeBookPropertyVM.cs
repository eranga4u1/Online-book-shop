using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class ChangeBookPropertyVM
    {
        public int bookId { get; set; }
        public int OldbookPropertyId { get; set; }
        public int NewpropertyId { get; set; }
    }
}