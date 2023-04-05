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

        public async Task<string> KokoRequest(KokoRequest model)
        {
            var kokoUrl = System.Configuration.ConfigurationManager.AppSettings["KokoUrl"];
            var kokoApiKey = System.Configuration.ConfigurationManager.AppSettings["kokoApiKey"];
            var kokoMid = System.Configuration.ConfigurationManager.AppSettings["kokoMid"];
            String dataString = model._mId + model._amount + model._currency + model._pluginName +
                                model._pluginVersion + model._returnUrl +
                                model._cancelUrl + model._orderId + model._reference +
                                model._firstName + model._lastName + model._email +
                                model._description + model.api_key + model._responseUrl;

            string signedData;
            string signature; ;
            GetSignInfo(dataString, "koko_private_key.txt", "koko_public_key.txt", out signedData, out signature);
            // GetSignInfo2(dataString, out signedData, out signature);
            var client = new RestClient("https://prodapi.paykoko.com/api/merchants/orderCreate");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("_mId", model._mId);
            request.AddParameter("api_key", kokoApiKey);
            request.AddParameter("_returnUrl", "https://localhost:44361/Payment/PaymentResponse?state=done");
            request.AddParameter("_cancelUrl", "https://localhost:44361/Payment/PaymentResponse?state=cancel");
            request.AddParameter("_responseUrl", "https://localhost:44361/Payment/PaymentResponse?state=notify");
            request.AddParameter("_amount", model._amount);
            request.AddParameter("_currency", "LKR");
            request.AddParameter("_reference", model._reference);
            request.AddParameter("_orderId", model._orderId);
            request.AddParameter("_pluginName", "customapi");
            request.AddParameter("_pluginVersion", "1");
            request.AddParameter("_description", string.Format("Online Muses Book Store :{0}",model._orderId) );
            request.AddParameter("_firstName", model._firstName);
            request.AddParameter("_lastName", model._lastName);
            request.AddParameter("_email",model._email);
            request.AddParameter("dataString", dataString);
            request.AddParameter("signature", signature);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }


        public static void SignData(string message, string privateKeyXml, out string signature, out string signedData)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Load the private key from XML
                rsa.FromXmlString(privateKeyXml);

                // Compute the hash of the message
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] hash = SHA256.Create().ComputeHash(messageBytes);

                // Sign the hash
                byte[] signatureBytes = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

                // Convert the signature and signed message to Base64 strings
                signature = Convert.ToBase64String(signatureBytes);
                signedData = Convert.ToBase64String(messageBytes);
            }
        }


        private static void GetSignInfo(string data, string privateKeyFileName, string publicKeyFileName, out string signedData, out string signature)
        {
            try
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                string privateKeyFile = Path.Combine(root, privateKeyFileName);
                string publicKeyFile = Path.Combine(root, publicKeyFileName);


                using (var rsa = new RSACryptoServiceProvider())
                {
                    // Load the private key from XML
                    rsa.FromXmlString(ReadTextFromFile(privateKeyFile));

                    // Compute the hash of the message
                    byte[] messageBytes = Encoding.UTF8.GetBytes(data);
                    byte[] hash = SHA256.Create().ComputeHash(messageBytes);

                    // Sign the hash
                    byte[] signatureBytes = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));

                    // Convert the signature and signed message to Base64 strings
                    signature = Convert.ToBase64String(signatureBytes);
                    signedData = Convert.ToBase64String(messageBytes);
                }

                //using (var rsa = new RSACryptoServiceProvider(1024))
                //{
                //    // Read the private key from the file
                //    string privateKeyXml = File.ReadAllText(privateKeyFile);
                //    rsa.FromXmlString(privateKeyXml);

                //    // Sign the data
                //    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                //    byte[] signedDataBytes = rsa.SignData(dataBytes, new SHA256CryptoServiceProvider());

                //    signedData = Convert.ToBase64String(signedDataBytes);

                //    // Read the public key from the file
                //    byte[] publicKeyBlob = File.ReadAllBytes(publicKeyFile);
                //    rsa.ImportCspBlob(publicKeyBlob);
                //    byte[] publicKey = rsa.ExportCspBlob(false);
                //    signature = Convert.ToBase64String(publicKey);
                //}


                Console.WriteLine("Signed Data: " + signedData);
                Console.WriteLine("Signature: " + signature);
            }
            catch(Exception ex)
            {
                signedData = "";
                signature = "";
                Console.WriteLine(ex.Message);
            }

        }
        private static void GetSignInfo2(string data, out string signedData, out string signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // Sign the data
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] signedDataBytes = rsa.SignData(dataBytes, new SHA256CryptoServiceProvider());

                signedData = Convert.ToBase64String(signedDataBytes);
                signature = Convert.ToBase64String(rsa.ExportCspBlob(false));
            }
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
                        discount = cart.Discount.ToString(),
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
    }
}
//https://kashifsoofi.github.io/cryptography/rsa-signing-in-csharp-using-microsoft-cryptography/
//https://blog.todotnet.com/2018/02/public-private-keys-and-signing/


//   public async Task<String> KokoRequest(KokoRequest model)
//   {
//       var  kokoUrl = System.Configuration.ConfigurationManager.AppSettings["KokoUrl"];
//       var kokoApiKey = System.Configuration.ConfigurationManager.AppSettings["kokoApiKey"];
//       var kokoMid = System.Configuration.ConfigurationManager.AppSettings["kokoMid"];
//       String dataString = model._mId + model._amount +model._currency+ model._pluginName +
//                           model._pluginVersion + model._returnUrl +
//                           model._cancelUrl+ model._orderId + model._reference+
//                           model._firstName + model._lastName + model._email +
//                           model._description + model.api_key + model._responseUrl;


//       string signedData;
//       string signature; ;
//       GetSignInfo(dataString, "koko_private_key.txt", "koko_public_key.txt",out signedData,out signature);
//      // GetSignInfo2(dataString, out signedData, out signature);

//       //bool comply = RsaVerify.verify(data, signature,merchantPluginDetail.getMerchantPublicKey());

//       if (!string.IsNullOrEmpty(kokoUrl) && !string.IsNullOrEmpty(kokoApiKey) && !string.IsNullOrEmpty(kokoMid))
//       {
//           model._mId = kokoMid;
//           model.api_key = kokoApiKey;
//           /* using (var client = new HttpClient())
//            {
//                var values = new Dictionary<string, string>
//                {
//                    {"_mId",model._mId},
//                    {"api_key", model.api_key },
//                    {"_returnUrl",model._returnUrl},
//                    {"_cancelUrl", model._cancelUrl},
//                    { "_responseUrl", model._responseUrl},
//                    { "_amount", model._amount},
//                    { "_currency",model._currency},
//                    { "_reference",model._reference},
//                    { "_orderId", model._orderId},
//                    { "_pluginName", model._pluginName},
//                    { "_pluginVersion", model._pluginVersion},
//                    { "_description", model._description},
//                    { "_firstName", model._firstName },
//                    { "_lastName", model._lastName},
//                    { "_email", model._email},
//                    { "dataString",signedData},
//                    {"signature",signature}
//                };



//                var content = new FormUrlEncodedContent(values);
//                try
//                {
//                    var response = await client.PostAsync(kokoUrl, content);

//                    var responseString = await response.Content.ReadAsStringAsync();
//                    return responseString;
//                }
//                catch(Exception ex)
//                {
//                    return "";
//                }


//            }*/
//var requestBody = new NameValueCollection
//                    {
//                        {"_mId", model._mId},
//                        {"api_key",  model.api_key },
//                        {"_returnUrl", model._returnUrl},
//                        {"_cancelUrl", model._cancelUrl},
//                        {"_responseUrl",model._responseUrl},
//                        {"_amount", model._amount},
//                        {"_currency", model._currency},
//                        {"_reference", model._reference},
//                        {"_orderId", model._orderId},
//                        {"_pluginName", model._pluginName},
//                        {"_pluginVersion", model._pluginVersion},
//                        {"_description", model._description},
//                        {"_firstName", model._firstName},
//                        {"_lastName", model._lastName},
//                        {"_email", model._email},
//                        {"dataString", signedData},
//                        {"signature", signature}
//                    };

//using (var client = new WebClient())
//{
//    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
//    try
//    {
//        var response = client.UploadValues(kokoUrl, "POST", requestBody);
//        Console.WriteLine(System.Text.Encoding.Default.GetString(response));
//        return response.ToString();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//        return ex.Message;
//    }

//}
//            }
//            else
//{
//    return null;
//}
           
//        }