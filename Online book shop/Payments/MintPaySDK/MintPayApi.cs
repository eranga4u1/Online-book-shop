using System;
using RestSharp;
using Online_book_shop.Payments.MintPaySDK.Models;
using Newtonsoft.Json;

namespace Online_book_shop.Payments.MintPaySDK
{
    public class MintPayApi
    {
        public MintPayResponse CreateOrder(MintPayRequestModel model, RequestMetaData metaData)
        {
            try
            {
                var client = new RestClient(metaData.EndPointURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", string.Format("Token {0}", metaData.Token));
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(model);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                return JsonConvert.DeserializeObject<MintPayResponse>(response.Content);
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public string RedirectToPayment(string PaymentId, string endpoint)
        {
            var client = new RestClient(endpoint);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("purchase_id", PaymentId);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.ResponseUri);
            return response.ResponseUri.ToString();
        }
    }
}