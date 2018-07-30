using System;
using System.Threading;

namespace BitCoinValueCheck
{
    class MainClass
    {
        static void Main()
        {
            //// Create an AutoResetEvent to signal the timeout threshold in the
            //// timer callback has been reached.
            var autoEvent = new AutoResetEvent(false);

            var statusChecker = new EachValueChecker();

            //// Create a timer that invokes CheckStatus after one second, 
            //// and every 1/4 second thereafter.
            //Console.WriteLine("{0:h:mm:ss.fff} Creating timer.\n",
            //                  DateTime.Now);
            var stateTimer = new Timer(statusChecker.CheckStatus,
                                       autoEvent, 10, 60000);
            //Console.ReadLine();

            // When autoEvent signals the second time, dispose of the timer.
            autoEvent.WaitOne();
            stateTimer.Dispose();
            //Console.WriteLine("\nDestroying timer.");
        }
    }

    class EachValueChecker
    {
        private BitCoinValueChecker bitFlyerValueChecker = new BitCoinValueChecker("001","/v1/ticker", "?product_code=BTC_JPY",
            "https://api.bitflyer.jp", "best_bid", "best_ask");

        private BitCoinValueChecker zaifValueChecker = new BitCoinValueChecker("002","/api/1/ticker/btc_jpy", "",
            "https://api.zaif.jp", "bid", "ask");

        // This method is called by the timer delegate.
        public void CheckStatus(Object stateInfo)
        {
            var nowtime = DateTime.Now;
            bitFlyerValueChecker.RunAsync(nowtime).GetAwaiter().GetResult();
            zaifValueChecker.RunAsync(nowtime).GetAwaiter().GetResult();
        }
    }
}