using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace HttpClientSample
{

    class ProgramZaif
    {
        static HttpClient client = new HttpClient();


        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static readonly Uri endpointUri = new Uri("https://api.zaif.jp");

        static async Task RunAsync()
        {
            var method = "GET";
            var path = "/api/1/ticker/btc_jpy";
//            var query = "?product_code=BTC_JPY";

            using (var client = new HttpClient())
//            using (var request = new HttpRequestMessage(new HttpMethod(method), path + query))
            using (var request = new HttpRequestMessage(new HttpMethod(method), path))
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