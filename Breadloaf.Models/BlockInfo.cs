using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Breadloaf.Models {
    public sealed class BlockInfo : IEquatable<BlockInfo> {
        [JsonPropertyName("nonce")]
        public long Nonce { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; private set; }

        [JsonPropertyName("previous-hash")]
        public string PreviousHash { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset TimeStamp { get; set; }

        [JsonPropertyName("transactions")]
        public IList<TransactionInfo> Transactions { get; set; }

        public bool Equals(BlockInfo other) {
            return Hash == other.Hash
                   && PreviousHash == other.PreviousHash
                   && Nonce == other.Nonce
                   && TimeStamp.Equals(other.TimeStamp)
                   && Equals(Transactions, other.Transactions);
        }

        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }

        public static bool operator ==(BlockInfo left, BlockInfo right) {
            return left.Equals(right);
        }

        public static bool operator !=(BlockInfo left, BlockInfo right) {
            return !left.Equals(right);
        }

        public void SetHash(string hash) {
            Hash = hash;
        }
    }
}