using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class SearchData
    {
        public int Id { get; set; }
        public int ObjectType { get; set; }
        public int objectId { get; set; }
        public string Title { get; set; }
        public string LocalTitle { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string ISBN { get; set; }
        //public string LocalDescription { get; set; }
    }
}