using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool isEnable { get; set; }
    }
}