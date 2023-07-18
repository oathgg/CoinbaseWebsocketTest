using CoinbaseWebsocketTest;

using Newtonsoft.Json;

using System.Security.Cryptography;
using System.Text;

BrokerClientWebSocket ws = new BrokerClientWebSocket("wss://advanced-trade-ws.coinbase.com", OnMessageReceived);
string msg = GenerateMessage(
    key: "xxxxx",
    secret: "xxxxx",
    productIds: new List<string> { "BTC-USDT", "BTC-USD", "BTC-USDC" },
    channelName: "user",
    unixTimeStamp: GetUnixTimeStamp());

ws.Connect();
ws.Send(msg);

Console.ReadKey();

static void OnMessageReceived(string message)
{
    var response = JsonConvert.DeserializeObject<StreamResponse>(message);
    if (response != null)
    {
        Console.WriteLine($"OnMessageReceived -> {response.Channel} - {response.Timestamp} - {response.SequenceNum}");
        switch (response.Channel)
        {
            case "ticker_batch":
            case "ticker":
                var tr = JsonConvert.DeserializeObject<TickerResponse>(message);
                if (tr is not null)
                {
                    foreach (TickerEvent e in tr.Events)
                    {
                        foreach (TickerData t in e.Tickers)
                        {
                            Console.WriteLine($"{t.ProductId} - {t.Price}");
                        }
                    }
                }
                break;
            case "user":
                var ur = JsonConvert.DeserializeObject<UserStreamResponse>(message);
                if (ur is not null)
                {
                    foreach (UserEvent e in ur.Events)
                    {
                        foreach (CoinbaseOrderStream o in e.Orders)
                        {
                            Console.WriteLine("User message received");
                        }
                    }
                }
                break;
                //case "subscriptions":
                //    var subResponse = JsonConvert.DeserializeObject<SubscriptionsResponse>(message);
                //    if (subResponse is not null)
                //    {
                //        foreach (var a in subResponse.Events)
                //        {
                //            if (a.Subscriptions.User is not null)
                //                Console.WriteLine("User: " + JsonConvert.SerializeObject(a.Subscriptions.User));
                //            if (a.Subscriptions.Level2 is not null)
                //                Console.WriteLine("Level2: " + JsonConvert.SerializeObject(a.Subscriptions.Level2));
                //            if (a.Subscriptions.Ticker is not null)
                //                Console.WriteLine("Ticker: " + JsonConvert.SerializeObject(a.Subscriptions.Ticker));
                //            if (a.Subscriptions.TickerBatch is not null)
                //                Console.WriteLine("TickerBatch: " + JsonConvert.SerializeObject(a.Subscriptions.TickerBatch));
                //        }
                //    }
                //    break;
                //case "heartbeats":
                //    var hbResp = JsonConvert.DeserializeObject<HeartbeatsStreamResponse>(message);
                //    break;
        }
    }
}

string GenerateMessage(string key, string secret, IEnumerable<string> productIds, string channelName, long unixTimeStamp)
{
    var str = JsonConvert.SerializeObject(new
    {
        type = "subscribe",
        product_ids = productIds,
        channel = channelName,
        signature = GenerateSignature(secret, $"{unixTimeStamp}{channelName}{string.Join(',', productIds)}"),
        api_key = key,
        timestamp = unixTimeStamp.ToString(),
    });

    return str;
}

static string GenerateSignature(string secret, string payload)
{
    // Create the HMAC-SHA256 object using the API secret key
    byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secret);
    byte[] messageBytes = Encoding.UTF8.GetBytes(payload);
    using var hmac = new HMACSHA256(secretKeyBytes);

    // Compute the hash
    byte[] hashBytes = hmac.ComputeHash(messageBytes);

    // Convert the hash to a hexadecimal string
    string signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

    return signature;
}

static long GetUnixTimeStamp()
{
    return ToUnixTimeStamp(DateTime.UtcNow.AddSeconds(-10));
}

static long ToUnixTimeStamp(DateTime dateTime)
{
    return Convert.ToInt64((TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);
}