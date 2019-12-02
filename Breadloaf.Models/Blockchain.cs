using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Breadloaf.Models {
    public sealed class Blockchain : IDisposable {
        [JsonPropertyName("transaction-threshold")]
        public int TransactionThreshold { get; set; } = 5;

        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; } = 2;

        [JsonPropertyName("nodes")]
        public IList<NodeInfo> Nodes { get; set; }

        [JsonPropertyName("blocks")]
        public IList<BlockInfo> Blocks { get; set; }

        [JsonPropertyName("pending-transactions")]
        public IList<TransactionInfo> PendingTransactions { get; set; }

        [JsonPropertyName("is-valid")]
        public bool IsValid
        {
            get
            {
                for (var i = 1; i < Blocks.Count; i++) {
                    var previous = Blocks[i - 1];
                    var current = Blocks[i];

                    var currentHash = Hashing.Get(current);
                    if (current.Hash != currentHash)
                        return false;

                    if (current.PreviousHash != previous.Hash)
                        return false;
                }

                return true;
            }
        }

        [JsonPropertyName("crumbs")]
        public double Crumbs
            => Blocks.Sum(x => x.Transactions.Sum(s => s.Amount));

        public event Func<BlockInfo, Task> OnBlockAdded;

        public Blockchain() {
            Nodes = new Collection<NodeInfo>();
            Blocks = new Collection<BlockInfo>();
            PendingTransactions = new Collection<TransactionInfo>();
        }

        public void CreateGenesisBlock() {
            var genesis = new BlockInfo {
                PreviousHash = "0",
                TimeStamp = DateTimeOffset.Now,
                Transactions = new Collection<TransactionInfo>()
            };

            Hashing.Create(ref genesis);
            Blocks.Add(genesis);
        }

        public void AddNode(NodeInfo node) {
            Nodes.Add(node);
        }

        public void RemoveNode(NodeInfo node) {
            Nodes.Remove(node);
        }

        public void AddBlock(BlockInfo block) {
            Blocks.Add(block);
            OnBlockAdded?.Invoke(block);
        }

        public void CreateTransaction(TransactionInfo transaction) {
            PendingTransactions.Add(transaction);

            if (PendingTransactions.Count < TransactionThreshold)
                return;

            var block = new BlockInfo {
                PreviousHash = Blocks[^1].Hash,
                TimeStamp = DateTimeOffset.Now,
                Transactions = PendingTransactions
            };

            var requiredZeros = new string('0', Difficulty);
            do {
                block.Nonce++;
                Hashing.Create(ref block);
            } while (string.IsNullOrWhiteSpace(block.Hash) || !block.Hash.StartsWith(requiredZeros));

            AddBlock(block);
        }

        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }

        public void Dispose() {
            Blocks.Clear();
            Nodes.Clear();
            PendingTransactions.Clear();
        }
    }
}