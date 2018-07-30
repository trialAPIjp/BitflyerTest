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

        private Uri endpointUri;           // API呼び出し用URLのサーバ名部分

        private readonly String bitString; // JsonでBIT値を表す文字列
        private readonly String askString; // JsonでASK値を表す文字列
        private readonly String companyID; // 業者ID 001:bitFlyer 002:Zaif
        private readonly String path;      // API呼び出し用URLのパス名部分
        private readonly String query;     // API呼び出し用URLのクエリ―部分

        // コンストラクタ
        // 引数で業者ごとにことなるAPI呼び出しに必要な情報を取得し、メンバ変数に設定
        public BitCoinValueChecker(
                            String p_companyID,
                            String p_path,
                            String p_query,
                            String urlString,
                            String p_bitString,
                            String p_askString)
        {
            companyID = p_companyID;
            bitString = p_bitString;
            askString = p_askString;
            path = p_path;
            query = p_query;

            // サーバ名についてはUrlオブジェクトを生成しておく
            endpointUri = new Uri(urlString);

        }

        public async Task RunAsync(DateTime nowtime)
        {

            using (var client = new HttpClient())
            using (request = new HttpRequestMessage(new HttpMethod("GET"), path + query))
            {
                client.BaseAddress = endpointUri;
                var message = await client.SendAsync(request);

                // ビットコイン価格情報取得APIを呼び出し
                var response = await message.Content.ReadAsStringAsync();
                // JSON形式に変換
                JObject s = JObject.Parse(response);

                // SQL Server LovalDBに接続する準備
                using (var context = new bitCoinTestEntities())
                {
                    // APIから取得した情報をテーブルに格納する
                    var datum = new BITCON_DATA()
                    {
                        QUERY_TIME = nowtime,
                        CUMPANY_ID = companyID,
                        BID = (decimal)s[bitString],
                        ASK = (decimal)s[askString],

                    };
                    context.BITCON_DATA.Add(datum);
                    context.SaveChanges();

                    /* 格納した値をコンソールに出力する */
                    // JSONデータをフォーマットする
                    var jsonString = JsonConvert.SerializeObject(datum, Formatting.Indented);
                    // コンソールに出力する
                    Console.WriteLine(jsonString);
                    // デバッグモードの場合は出力ウィンドウにも出力する
                    System.Diagnostics.Debug.WriteLine(datum);
                }

            }
        }


    }
}