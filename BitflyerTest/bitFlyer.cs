using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BitflyerTest;



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
                JObject s = JObject.Parse(response);
                string product_code = (string)s["product_code"];
                decimal bid = (decimal)s["best_bid"];
                Console.WriteLine(bid);
                System.Diagnostics.Debug.WriteLine(bid);

                using (var context = new bitCoinTestEntities())
                {
                    var datum = new BITCON_DATA()
                    {
                        QUERY_TIME = DateTime.Now,
                        CUMPANY_ID = "001",
                        BID = (decimal)s["best_bid"],
                        ASK = (decimal)s["best_ask"],

                    };
                    context.BITCON_DATA.Add(datum);
                    context.SaveChanges();
                }

            }
        }


    }
}