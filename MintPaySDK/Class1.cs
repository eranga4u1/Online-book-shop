using MintPaySDK.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json.Serialization;


namespace MintPaySDK
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
                request.AddHeader("Authorization", string.Format("Token {0}",metaData.Token));
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(model);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                return JsonConvert.DeserializeObject<MintPayResponse>(response.Content);
            }
            catch(Exception exception)
            {
                throw exception;
            }

        }
    }
}