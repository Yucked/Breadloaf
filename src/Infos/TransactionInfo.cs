using System;
using System.Net;
using System.Text.Json.Serialization;
using Breadloaf.Converters;

namespace Breadloaf.Infos {
    public struct TransactionInfo {
        [JsonPropertyName("from"), JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint From { get; set; }

        [JsonPropertyName("to"), JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint To { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
        
        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}