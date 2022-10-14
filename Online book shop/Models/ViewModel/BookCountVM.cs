using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class BookCountVM
    {
        public int BookId { get; set; }
        public int Count { get; set; }

        public string BookName { get; set; }
        public string BookPropertyName { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}