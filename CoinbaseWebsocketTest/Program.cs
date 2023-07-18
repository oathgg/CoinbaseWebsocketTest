using CoinbaseWebsocketTest;

using Newtonsoft.Json;

using System.Security.Cryptography;
using System.Text;

BrokerClientWebSocket ws = new BrokerClientWebSocket("wss://advanced-trade-ws.coinbase.com", OnMessageReceived);
string msg = GenerateMessage(
    key: "xxxxx",
    secret: "xxxxxx",
    productIds: new List<string> { "BTC-USDT", "BTC-USD", "BTC-USDC" },
    channelName: "user",
    unixTimeStamp: GetUnixTimeStamp());

ws.Connect();
ws.Send(msg);

Console.ReadKey();

static void OnMessageReceived(string message)
{
    Console.WriteLine(message);
}

static string GenerateMessage(string key, string secret, IEnumerable<string> productIds, string channelName, long unixTimeStamp)
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