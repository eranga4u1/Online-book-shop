using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class DataObjVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ObjType { get; set; }
        public string OtherPara { get; set; }
        public int BookAuthorId { get; set; }
    }
}