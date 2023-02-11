using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_book_shop.Models
{
    public class KokoRequest
    {
        public string _mId { get; set; }
        public string api_key { get; set; }
        public string _returnUrl { get; set; }
        public string _cancelUrl { get; set; }
        public string _responseUrl { get; set; }
        public string _amount { get; set; }
        public string _currency { get; set; }
        public string _reference { get; set; }
        public string _orderId { get; set; }
        public string _pluginName { get; set; }
        public string _pluginVersion { get; set; }
        public string _description { get; set; }
        public string _firstName { get; set; }
        public string _lastName { get; set; }
        public string _email { get; set; }
        public string dataString { get; set; }
        public string signature { get; set; }
    }
}