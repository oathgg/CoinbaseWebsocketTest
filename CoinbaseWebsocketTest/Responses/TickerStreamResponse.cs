﻿using Newtonsoft.Json;

namespace CoinbaseWebsocketTest
{
    public class TickerResponse
    {
        [JsonProperty("events")]
        public List<TickerEvent> Events { get; set; }
    }

    public class TickerEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tickers")]
        public List<TickerData> Tickers { get; set; }
    }

    public class TickerData
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("volume_24_h")]
        public string Volume24H { get; set; }

        [JsonProperty("low_24_h")]
        public string Low24H { get; set; }

        [JsonProperty("high_24_h")]
        public string High24H { get; set; }

        [JsonProperty("low_52_w")]
        public string Low52W { get; set; }

        [JsonProperty("high_52_w")]
        public string High52W { get; set; }

        [JsonProperty("price_percent_chg_24_h")]
        public string PricePercentChg24H { get; set; }
    }
}
