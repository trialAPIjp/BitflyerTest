using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoinValueCheck
{
    using System;
    using System.Timers;

    public class MainClass
    {
        private static System.Timers.Timer aTimer;

        private static BitCoinValueChecker bitFlyerValueChecker;
        private static BitCoinValueChecker zaifValueChecker;

        public static void Main()
        {
            // 開始したことをコンソールに出力
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);

            // bitFlyer用の価格チェッククラスのインスタンス
            bitFlyerValueChecker = new BitCoinValueChecker("001", "/v1/ticker", "?product_code=BTC_JPY",
            "https://api.bitflyer.jp", "best_bid", "best_ask");

            // Zaif用の価格チェッククラスのインスタンス
            zaifValueChecker = new BitCoinValueChecker("002", "/api/1/ticker/btc_jpy", "",
            "https://api.zaif.jp", "bid", "ask");

            //初回API呼び出し(プログラムの動作確認用)
            CallBitCoinAPI();

            // タイマー開始
            SetTimer();

            // 何か入力されたらプログラムを停止する
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer()
        {
            // 60秒間隔を設定
            aTimer = new System.Timers.Timer(60000);
            // 指定した時間が経過したら呼ばれるメソッドを設定 
            aTimer.Elapsed += OnTimedEvent;
            // Timerが繰り返し実行される
            aTimer.AutoReset = true;
            // Timerを実行する
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // ビットコイン価格取得API呼び出し
            CallBitCoinAPI();
        }

        private static void CallBitCoinAPI()
        {
            // 現在時刻を設定
            var nowtime = DateTime.Now;
            // bitFlyerのAPIを呼び出す
            bitFlyerValueChecker.RunAsync(nowtime).GetAwaiter().GetResult();
            // ZaifのAPIを呼び出す
            zaifValueChecker.RunAsync(nowtime).GetAwaiter().GetResult();
        }
    }
}
