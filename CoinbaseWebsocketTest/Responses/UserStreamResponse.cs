using Newtonsoft.Json;

namespace CoinbaseWebsocketTest
{
    public class UserStreamResponse : StreamResponse
    {
        [JsonProperty("events")]
        public List<UserEvent> Events { get; set; }
    }

    public class UserEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("orders")]
        public List<CoinbaseOrderStream> Orders { get; set; }
    }

    public class CoinbaseOrderStream
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("client_order_id")]
        public string ClientOrderId { get; set; }

        [JsonProperty("cumulative_quantity")]
        public string CumulativeQuantity { get; set; }

        [JsonProperty("leaves_quantity")]
        public string LeavesQuantity { get; set; }

        [JsonProperty("avg_price")]
        public string AvgPrice { get; set; }

        [JsonProperty("total_fees")]
        public string TotalFees { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("creation_time")]
        public DateTime CreationTime { get; set; }

        [JsonProperty("order_side")]
        public string OrderSide { get; set; }

        [JsonProperty("order_type")]
        public string OrderType { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        [JsonProperty("reject_Reason")]
        public string RejectReason { get; set; }
    }
}
