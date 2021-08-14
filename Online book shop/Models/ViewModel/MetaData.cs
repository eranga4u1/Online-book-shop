using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class MetaData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; } = "Muses Publishing House";
        public string Keywords { get; set; }       
        public string Robots { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
    }
}