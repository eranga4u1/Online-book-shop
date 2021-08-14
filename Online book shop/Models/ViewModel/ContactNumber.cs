using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Models.ViewModel
{
    public class ContactNumber
    {
        public string NumberValue { get; set; }
        public bool ShowsOnContactPage { get; set; }
        public bool ShowsOnHomePage { get; set; }
        public string TitleForNumber { get; set; }
    }
    public class EmailAddress
    {
        public string EmailValue { get; set; }
        public bool ShowsOnContactPage { get; set; }
        public bool ShowsOnHomePage { get; set; }
        public string TitleForEmail { get; set; }
    }
    public class MusesAddress
    {
        public string AddressValue { get; set; }
        public bool ShowsOnContactPage { get; set; }
        public bool ShowsOnHomePage { get; set; }
        public string TitleForAddress { get; set; }
    }

    public class PaymentMethod
    {
        public int EnumId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public   bool isEnable { get; set; } 
    }

    public class InstantMessage
    {
        [AllowHtml]
        public string Message { get; set; }
        public bool isEnable { get; set; } = true;
        public string FontColor { get; set; }
        public string BackgroundColor { get; set; }
    }
}