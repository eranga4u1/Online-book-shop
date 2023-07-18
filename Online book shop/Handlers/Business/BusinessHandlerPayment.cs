using iTextSharp.text.pdf.codec.wmf;
using Newtonsoft.Json;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Online_book_shop.Payments.MintPaySDK;
using Online_book_shop.Payments.MintPaySDK.Models;
using MongoDB.Driver.Core.Authentication.Vendored;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName;

namespace Online_book_shop.Handlers.Business
{
    public class BusinessHandlerPayment
    {

        public   async Task<string> PaymentRequestAsync(PayHereRequest model)
        {
            try
            {
                model.merchant_id = System.Configuration.ConfigurationManager.AppSettings["MerchantId"];
                string payhereSecret = CryptoHelper.CreateMD5(System.Configuration.ConfigurationManager.AppSettings["MerchantSecret"]).ToUpper();
                string hash = string.Format("{0}{1}{2}{3}{4}", model.merchant_id, model.order_id, model.amount, model.currency, payhereSecret);
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
                testAsync(model: model);
                // string url = System.Configuration.ConfigurationManager.AppSettings["PayHereUrl"];
                //var data = new FormUrlEncodedContent(values);
                //var response = await client.PostAsync(url, data);
                return null;// await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
      
        }

        public async Task testAsync(PayHereRequest model)
        {
            using (var client = new HttpClient())
            {
                // Set the base address and configure the client
                client.BaseAddress = new Uri("https://sandbox.payhere.lk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Build the request content
                
                    

                // Send the POST request
                HttpResponseMessage response = await client.PostAsync("/pay/checkout", GetJsonPayLoad(model));

                // Read the response
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            }
        }

        public StringContent GetJsonPayLoad(PayHereRequest model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model),
                System.Text.Encoding.UTF8, "application/json");

            return content;
        }

        public string KokoRequest(KokoRequest model)
        {

            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            model.baseUrl = baseUrl;
            var kokoUrl = "https://prodapi.paykoko.com/api/merchants/orderCreate";
            var kokoApiKey = "SVL5WxQ1i7Hq3M2foIv5kgUVyTMAZQ03";
            var kokoMid = "f45592f363a1647f44ee688185cf8d43";
            var returnUrl = string.Format("{0}Payment/PaymentResponse?state=done&order_id={1}", baseUrl, model._orderId);
            var cancelUrl = string.Format("{0}Payment/PaymentResponse?state=cancel&order_id={1}", baseUrl, model._orderId); 
            var responseUrl = string.Format("{0}Payment/PaymentResponse?state=notify&order_id={1}", baseUrl, model._orderId); 
            var _pluginName = "customapi";
            var _pluginVersion = "1";
            var description = model._description= string.Format("Online Muses Book Store :{0}", model._orderId);

            String dataString = kokoMid + model._amount + model._currency + _pluginName +
                                _pluginVersion + returnUrl +
                                cancelUrl + model._orderId + model._reference +
                                model._firstName + model._lastName + model._email +
                                 description + kokoApiKey + responseUrl;



            string signature = SignStringWithRSA(model); // SignStringWithRSA(dataString);//

            BusinessHandlerMPLog.Log(LogType.Message, String.Format("Data String for{0}: {1}", model._orderId, dataString), "BusinessHandlerPayment", "KokoRequest",null);
            BusinessHandlerMPLog.Log(LogType.Message, String.Format("signature for{0}: {1}", model._orderId, signature), "BusinessHandlerPayment", "KokoRequest", null);

            //string signature = "OTnomXksafe6ZdICi/pMEpRnXj54e5Qy5dxQ5O/oXYsi8vxAicKBAvgwLAmUYuMuTH6Kk4ltst/6hiHzmLZpn/8xb1EfTCIYYAf/mdUkqAjNDbyCNIiZPRBQIWTkBX0QeKkReyDBJAm6m2uv1QZWbQLoltNpH5pDvqZ/CgreKds=";

            var client = new RestClient(kokoUrl);
          
            client.Timeout = -1;
            var request1 = new RestRequest(Method.POST);
            request1.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request1.AddParameter("_mId", kokoMid);
            request1.AddParameter("api_key", kokoApiKey);
            request1.AddParameter("_returnUrl", returnUrl);
            request1.AddParameter("_cancelUrl", cancelUrl);
            request1.AddParameter("_responseUrl", responseUrl);
            request1.AddParameter("_amount", model._amount);
            request1.AddParameter("_currency", "LKR");
            request1.AddParameter("_reference", model._reference);
            request1.AddParameter("_orderId", model._orderId);
            request1.AddParameter("_pluginName", _pluginName);
            request1.AddParameter("_pluginVersion", _pluginVersion);
            request1.AddParameter("_description", description);
            request1.AddParameter("_firstName", model._firstName);
            request1.AddParameter("_lastName", model._lastName);
            request1.AddParameter("_email", model._email);
            request1.AddParameter("dataString", dataString);
            request1.AddParameter("signature", signature);
            IRestResponse response = client.Execute(request1);

            BusinessHandlerMPLog.Log(LogType.Message, String.Format("Request for{0}: {1}", model._orderId, JsonConvert.SerializeObject(request1)), "BusinessHandlerPayment", "KokoRequest", null);

            BusinessHandlerMPLog.Log(LogType.Message, String.Format("Response for{0}: {1}", model._orderId, JsonConvert.SerializeObject(response)), "BusinessHandlerPayment", "KokoRequest", null);

            Console.WriteLine(response.Content);
            return response.ResponseUri.AbsoluteUri;
        }




        public static Cart UpdateCartForKokoPayment(int cartId)
        {
            return DBHandlerPayments.UpdateCartForKokoPayment(cartId);
        }
        public static Cart RevertCartFromKokoPayment(int cartId)
        {
            return DBHandlerPayments.RevertCartFromKokoPayment(cartId);
        }
        public static string ReadTextFromFile(string path)
        {
            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(path))
                {
                    // Read the entire file and store its contents in a string.
                    string fileContents = sr.ReadToEnd();

                    return fileContents;

                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static MintPayResponse MintPayPayment(ProcessOrder model)
        {
            if(model != null)
            {
                Order order = BusinessHandlerOrder.GetByUID(model.OrderId);
                Cart cart = BusinessHandlerShopingCart.GetById(model.CartId);
                string domain=  System.Configuration.ConfigurationManager.AppSettings["AppDomain"];
                MintPayRequestModel mintPayRequestModel = new MintPayRequestModel();
                RequestMetaData metaData = new RequestMetaData
                {
                    EndPointURL = System.Configuration.ConfigurationManager.AppSettings["MintPayEndPoint"],
                    Token = System.Configuration.ConfigurationManager.AppSettings["MintPayToken"]
                };

                mintPayRequestModel.merchant_id = System.Configuration.ConfigurationManager.AppSettings["MintPayMerchantId"];
                mintPayRequestModel.order_id = order.UId;
                mintPayRequestModel.total_price = model.TotalAmount.ToString();
                mintPayRequestModel.cart_created_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mintPayRequestModel.cart_updated_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mintPayRequestModel.discount = "0.0";
                mintPayRequestModel.customer_email = order.EmailAddress;
                mintPayRequestModel.customer_id = order.Id.ToString();
                mintPayRequestModel.customer_telephone = order.ContactNumber;
                mintPayRequestModel.delivery_street = order.DeliveryAddress;
                mintPayRequestModel.delivery_region = order.DeliveryAddressId.ToString();
                mintPayRequestModel.success_url = string.Format("{0}Payment/PaymentResponse?state=done&order_id={1}", domain,order.UId);
                mintPayRequestModel.fail_url = string.Format("{0}Payment/PaymentResponse?state=cancel&order_id={1}", domain, order.UId);
                mintPayRequestModel.ip = "13.67.91.149";
                mintPayRequestModel.x_forwarded_for = "13.67.91.149";

                if(cart != null)
                {
                    mintPayRequestModel.products = new List<Product>();
                    Product p = new Product
                    {
                        name = "Muses Book Cart",
                        product_id = cart.Id.ToString(),
                        sku = cart.Id.ToString(),
                        quantity = "1",
                        unit_price = cart.AmountAfterDiscount.ToString(),
                        discount = "0.0",
                        created_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        updated_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    mintPayRequestModel.products.Add(p);
                }


                MintPayApi api = new MintPayApi();
                MintPayResponse response = api.CreateOrder(mintPayRequestModel, metaData);
                if(response !=null && response.message== "Success")
                {
                   // var RedirectEndPointURL = System.Configuration.ConfigurationManager.AppSettings["MintPayLoginEndpoint"];
                   // var rslt= api.RedirectToPayment(response.data, RedirectEndPointURL);
                    return new MintPayResponse { data = response.data, message = "abc" };
                }
                else
                {
                    return new MintPayResponse { data = "ProcessOrder null", message = "Payment creation Id failed" };
                }
            }
            else
            {
                return new MintPayResponse { data = "ProcessOrder null", message = "Failed mint pay call" };
            }
          
        }

        private static string SignStringWithRSA(KokoRequest model)
        {
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["PaymentAPI"];
            var client = new RestClient(String.Format("{0}api/Koko/Post", baseUrl));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body =JsonConvert.SerializeObject(model);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
        private static string SignStringFromDataString(string datastring)
        {
            var client = new RestClient("http://localhost:5188/Koko?DataString=" + datastring);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
        public static string SignStringWithRSA(string stringToSign)
        {
            string pemPrivateKey = "-----BEGIN RSA PRIVATE KEY-----" +
                    "MIICWgIBAAKBgHYtAJezj90UF6mUfvajDXd0oZG1J3/LA/iXakBDXxegkgHiSyIh" +
                    "ZdsRgZNa4aSlF1lEMmQJjaEP6CJv6PUDmCz1f8spGrT1JXGBiv54nrnPa87q68oU" +
                    "8zt3B8UgAUBkM1EfKH2s7FPbQRs43oCVh6KmtaIvb/yCPoL+P3n1ybnLAgMBAAEC" +
                    "gYAkoF4Gpoh4JLoQvQ18s5yA4Y0R8+uCGBHrAkLUGA1o7UNTgid3NJK1Cv/2A7zb" +
                    "oq9R42kayDs1KBDyW20AQ1TubdYZW+u3svxh+vnpbqS6RGLI7pLJ8PkUDWqRLMFP" +
                    "G3cDgfAOzT2+fuaBzK84sdr5nW1dpydecXdAPEOZVmbOAQJBAMHF5DaBywAqrKb6" +
                    "tALUXUzkrmAiUPYNRYq7kb0x5KfVCg+go7Ecg7HNsHlljfGrEqcXyo36/Sb2jHY5" +
                    "gi8yuCECQQCcIEB05xwvaGgABbLo9U1fLLgRcfLfVtLaq5nVuLlx4XdyGDoBTMqo" +
                    "obATpnQweBhPyXYsvJ1xZCxUJl0U7kRrAkATCcVlUZVHW+oAsesTyBeuoV08lsKL" +
                    "mjw16D3mb8t+beECLg9HLH0H8CShmMe8cclwX1cIYhuTQ3ADgZz31CzhAkAo2Ttk" +
                    "Gs/CC6QiVVthHkVXIIEsd07fZn0Wn41JYOKMTDyPSo1qp6fihSNnkMaXo+Rgg8p6" +
                    "nALplxcOEVeLUWfvAkB/C78+QyxX63NT7OcuZk5lBFb6FfzLqZxvK7Ll5t2qiQE7" +
                    "c0tIgd04L68mOE4QmIrmio81v1vip/HwRnfhCl75" +
                    "-----END RSA PRIVATE KEY-----";

            // Load the RSA private key from the PEM string using BouncyCastle
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(GetPrivateKeyFromPem(pemPrivateKey));

            // Convert the string to sign to bytes
            byte[] data = Encoding.UTF8.GetBytes(stringToSign);

            // Compute the signature
            byte[] signature = rsa.SignData(data, new SHA256CryptoServiceProvider());

            // Convert the signature to base64 string
            string base64Signature = Convert.ToBase64String(signature);

            return base64Signature;
        }

        private static RSAParameters GetPrivateKeyFromPem(string pemPrivateKey)
        {
            // Extract the private key from the PEM string
            var privateKeyBytes = Convert.FromBase64String(pemPrivateKey);
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(privateKeyBytes);

            return rsa.ExportParameters(true);
        }

    }
}
//https://kashifsoofi.github.io/cryptography/rsa-signing-in-csharp-using-microsoft-cryptography/
//https://blog.todotnet.com/2018/02/public-private-keys-and-signing/


