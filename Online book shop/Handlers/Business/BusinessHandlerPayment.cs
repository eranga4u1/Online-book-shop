using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models.ViewModel;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerPayment
    {
        public async Task<string> PaymentRequestAsync(PayHereRequest model)
        {
            try
            {
                model.merchant_id = System.Configuration.ConfigurationManager.AppSettings["MerchantId"];
                string payhereSecret = CryptoHelper.CreateMD5(System.Configuration.ConfigurationManager.AppSettings["MerchantSecret"]).ToUpper();
                string hash = string.Format("{0}{1}{2}{3}{4}{5}", model.merchant_id, model.order_id, model.amount, model.currency, payhereSecret);
                model.hash = CryptoHelper.CreateMD5(hash).ToUpper();

                HttpClient client = new HttpClient();
                var values = new Dictionary<string, string>
                {
                    { "merchant_id", model.merchant_id},
                    { "return_url", model.return_url },
                    { "cancel_url", model.cancel_url },
                    { "notify_url", model.notify_url },
                    { "first_name", model.first_name },
                    { "last_name", model.last_name },
                    { "email", model.email },
                    { "phone", model.phone },
                    { "city", model.city },
                    { "country", model.country },
                    { "order_id", model.order_id },
                    { "items", model.items },
                    { "currency", model.currency },
                    { "amount", model.amount },
                    { "hash", model.hash }
                };

                string url = System.Configuration.ConfigurationManager.AppSettings["PayHereUrl"];
                var data = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, data);
                return await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
      
        }
    }
}