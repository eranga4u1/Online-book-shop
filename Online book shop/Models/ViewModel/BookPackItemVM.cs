using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BookPackItemVM
    {
        public int BookId { get; set; }
        public int PropertyId { get; set; }
        public int NumberOfBookPackItems { get; set; }

        public bool isAvailableForBookPackCreation { get; set; }
        public string Message { get; set; }
    }
}