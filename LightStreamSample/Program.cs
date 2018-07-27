﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Authentication;
using WebSocket4Net;

namespace LightStreamSample
{
    class WebSocket4NetSample
    {
        static void Main(string[] args)
        {
            var channelName = "lightning_ticker_BTC_JPY";
            // note: reconnection handling needed.
            var websocket = new WebSocket("wss://ws.lightstream.bitflyer.com/json-rpc", sslProtocols: SslProtocols.Tls12);
            websocket.Opened += (sender, e) =>
            {
                websocket.Send(
                    JsonConvert.SerializeObject(
                        new
                        {
                            method = "subscribe",
                            @params = new { channel = channelName },
                            id = 123,
                        }
                    )
                );
            };
            websocket.MessageReceived += (sender, e) =>
            {
                dynamic data = JObject.Parse(e.Message);
                if (data.id == 123)
                {
                    Console.WriteLine("subscribed!");
                }
                if (data.@params != null)
                {
                    Console.WriteLine(data.@params.channel + " " + data.@params.message);
                }
            };

            websocket.Open();

            Console.ReadKey();
        }
    }
}