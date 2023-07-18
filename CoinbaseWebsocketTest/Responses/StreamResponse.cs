using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinbaseWebsocketTest
{
    public class StreamResponse
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("sequence_num")]
        public int SequenceNum { get; set; }
    }
}
