using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Breadloaf.Infos {
#pragma warning disable 660,661
    public class BlockInfo : IEquatable<BlockInfo> {
        [JsonPropertyName("hash")]
        public string Hash { get; internal set; }

        [JsonPropertyName("previous-hash")]
        public string PreviousHash { get; internal set; }

        [JsonPropertyName("proof")]
        public int Proof { get; internal set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset TimeStamp { get; internal set; }

        [JsonPropertyName("transactions")]
        public IList<TransactionInfo> Transactions { get; internal set; }

        public bool Equals(BlockInfo other) {
            return Hash == other.Hash
                   && PreviousHash == other.PreviousHash
                   && Proof == other.Proof
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
#pragma warning restore 660,661
    }
}