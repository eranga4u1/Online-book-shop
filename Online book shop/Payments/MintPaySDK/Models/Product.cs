using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_book_shop.Payments.MintPaySDK.Models
{
    public class Product
    {
        public string name { get; set; }
        public string product_id { get; set; }
        public string sku { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public string discount { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
    }
}
