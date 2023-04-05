using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_book_shop.Payments.MintPaySDK.Models
{
    public class MintPayRequestModel
    {
        public string merchant_id { get; set; }
        public string order_id { get; set; }
        public string total_price { get; set; }
        public string discount { get; set; }
        public string customer_email { get; set; }
        public string customer_id { get; set; }
        public string customer_telephone { get; set; }
        public string ip { get; set; }
        public string x_forwarded_for { get; set; }
        public string delivery_street { get; set; }
        public string delivery_region { get; set; }
        public string delivery_postcode { get; set; }
        public string cart_created_date { get; set; }
        public string cart_updated_date { get; set; }
        public List<Product> products { get; set; }
        public string success_url { get; set; }
        public string fail_url { get; set; }
    }
}
