using Newtonsoft.Json;
using Online_book_shop.Handlers.Database;
using Online_book_shop.Handlers.Helper;
using Online_book_shop.Models;
using Online_book_shop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<String> KokoRequest(KokoRequest model)
        {
            var  kokoUrl = System.Configuration.ConfigurationManager.AppSettings["KokoUrl"];
            var kokoApiKey = System.Configuration.ConfigurationManager.AppSettings["kokoApiKey"];
            var kokoMid = System.Configuration.ConfigurationManager.AppSettings["kokoMid"];
            String dataString = model._mId + model._amount +model._currency+ model._pluginName +
                                model._pluginVersion + model._returnUrl +
                                model._cancelUrl+ model._orderId + model._reference+
                                model._firstName + model._lastName + model._email +
                                model._description + model.api_key + model._responseUrl;


            string signedData;
            string signature; ;
            GetSignInfo(dataString, "koko_private_key.txt", "koko_public_key.txt",out signedData,out signature);
           // GetSignInfo2(dataString, out signedData, out signature);

            //bool comply = RsaVerify.verify(data, signature,merchantPluginDetail.getMerchantPublicKey());

            if (!string.IsNullOrEmpty(kokoUrl) && !string.IsNullOrEmpty(kokoApiKey) && !string.IsNullOrEmpty(kokoMid))
            {
                model._mId = kokoMid;
                model.api_key = kokoApiKey;
                /* using (var client = new HttpClient())
                 {
                     var values = new Dictionary<string, string>
                     {
                         {"_mId",model._mId},
                         {"api_key", model.api_key },
                         {"_returnUrl",model._returnUrl},
                         {"_cancelUrl", model._cancelUrl},
                         { "_responseUrl", model._responseUrl},
                         { "_amount", model._amount},
                         { "_currency",model._currency},
                         { "_reference",model._reference},
                         { "_orderId", model._orderId},
                         { "_pluginName", model._pluginName},
                         { "_pluginVersion", model._pluginVersion},
                         { "_description", model._description},
                         { "_firstName", model._firstName },
                         { "_lastName", model._lastName},
                         { "_email", model._email},
                         { "dataString",signedData},
                         {"signature",signature}
                     };



                     var content = new FormUrlEncodedContent(values);
                     try
                     {
                         var response = await client.PostAsync(kokoUrl, content);

                         var responseString = await response.Content.ReadAsStringAsync();
                         return responseString;
                     }
                     catch(Exception ex)
                     {
                         return "";
                     }


                 }*/
                var requestBody = new NameValueCollection
                    {
                        {"_mId", model._mId},
                        {"api_key",  model.api_key },
                        {"_returnUrl", model._returnUrl},
                        {"_cancelUrl", model._cancelUrl},
                        {"_responseUrl",model._responseUrl},
                        {"_amount", model._amount},
                        {"_currency", model._currency},
                        {"_reference", model._reference},
                        {"_orderId", model._orderId},
                        {"_pluginName", model._pluginName},
                        {"_pluginVersion", model._pluginVersion},
                        {"_description", model._description},
                        {"_firstName", model._firstName},
                        {"_lastName", model._lastName},
                        {"_email", model._email},
                        {"dataString", signedData},
                        {"signature", signature}
                    };

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    try
                    {
                        var response = client.UploadValues(kokoUrl, "POST", requestBody);
                        Console.WriteLine(System.Text.Encoding.Default.GetString(response));
                        return response.ToString();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return ex.Message;
                    }
                   
                }
            }
            else
            {
                return null;
            }
           
        }



        private static void GetSignInfo(string data, string privateKeyFileName, string publicKeyFileName, out string signedData, out string signature)
        {
            try
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                string privateKeyFile = Path.Combine(root, privateKeyFileName);
                string publicKeyFile = Path.Combine(root, publicKeyFileName);

                using (var rsa = new RSACryptoServiceProvider(1024))
                {
                    // Read the private key from the file
                    string privateKeyXml = File.ReadAllText(privateKeyFile);
                    rsa.FromXmlString(privateKeyXml);

                    // Sign the data
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] signedDataBytes = rsa.SignData(dataBytes, new SHA256CryptoServiceProvider());

                    signedData = Convert.ToBase64String(signedDataBytes);

                    // Read the public key from the file
                    byte[] publicKeyBlob = File.ReadAllBytes(publicKeyFile);
                    rsa.ImportCspBlob(publicKeyBlob);
                    byte[] publicKey = rsa.ExportCspBlob(false);
                    signature = Convert.ToBase64String(publicKey);
                }


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
    }
}
//https://kashifsoofi.github.io/cryptography/rsa-signing-in-csharp-using-microsoft-cryptography/
//https://blog.todotnet.com/2018/02/public-private-keys-and-signing/