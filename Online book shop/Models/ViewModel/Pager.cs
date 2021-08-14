using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class Pager
    {
        public PagerItem Prev { get; set; }
        public PagerItem Current { get; set; }
        public PagerItem Next { get; set; }
        public List<int> LinkedPages { get; set; }
        public int NumberOfPage { get; set; }

    }
    public class PagerItem
    {
        public int LinkPageId { get; set; }
        public bool isEnable{get;set;}
    }
}