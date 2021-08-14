using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models.ViewModel
{
    public class MenuVM
    {
        public string Title { get; set; }
        public string LocalTitle { get; set; }
        public string Url { get; set; }
        public string ActiveFor { get; set; }
        public List<MenuVM> SubMenus { get; set;}
        [NotMapped]
        public string Parent{ get; set; }
    }
    public class MenuItem
    {
        public string text { get; set; }
        public string href { get; set; }
        public string icon { get; set; }
        public string target { get; set; }
        public string title { get; set; }
        //public string Localtitle { get; set; }
        public List<MenuItem> children { get; set; }

    }
}