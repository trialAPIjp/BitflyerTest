using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;



namespace BitCoinValueCheck
{

    public class BitCoinValueChecker
    {
        private readonly HttpClient client = new HttpClient();

        private HttpRequestMessage request;

        private Uri endpointUri;

        private readonly String bitString;
        private readonly String askString;
        private readonly String companyID;
        private readonly String path;
        private readonly String query;



        public BitCoinValueChecker(
                            String p_companyID,
                            String p_path,
                            String p_query,
                            String urlString,
                            String p_bitString,
                            String p_askString)
        {
            companyID = p_companyID;
            endpointUri = new Uri(urlString);
            bitString = p_bitString;
            askString = p_askString;
            path = p_path;
            query = p_query;

        }

        public async Task RunAsync(DateTime nowtime)
        {

            using (var client = new HttpClient())
            using (request = new HttpRequestMessage(new HttpMethod("GET"), path + query))
            {
                client.BaseAddress = endpointUri;
                var message = await client.SendAsync(request);
                var response = await message.Content.ReadAsStringAsync();
                JObject s = JObject.Parse(response);

                using (var context = new bitCoinTestEntities())
                {
                    var datum = new BITCON_DATA()
                    {
                        QUERY_TIME = nowtime,
                        CUMPANY_ID = companyID,
                        BID = (decimal)s[bitString],
                        ASK = (decimal)s[askString],

                    };
                    context.BITCON_DATA.Add(datum);
                    context.SaveChanges();
                    var jsonString = JsonConvert.SerializeObject(datum, Formatting.Indented);
                    Console.WriteLine(jsonString);
                    System.Diagnostics.Debug.WriteLine(jsonString);
                }

            }
        }


    }
}