using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace HttpClientSample
{

    class Program
    {
        static HttpClient client = new HttpClient();


        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static readonly Uri endpointUri = new Uri("https://api.bitflyer.jp");

        static async Task RunAsync()
        {
            var method = "GET";
            var path = "/v1/ticker";
            var query = "?product_code=BTC_JPY";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(new HttpMethod(method), path + query))
            {
                client.BaseAddress = endpointUri;
                var message = await client.SendAsync(request);
                var response = await message.Content.ReadAsStringAsync();

                Console.WriteLine(response);
                System.Diagnostics.Debug.WriteLine(response);
            }
        }


    }
}