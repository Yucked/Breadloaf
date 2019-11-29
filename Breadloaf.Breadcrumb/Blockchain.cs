using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Breadloaf.Breadcrumb {
    public sealed class Blockchain {
        [JsonPropertyName("nodes")]
        public IList<NodeInfo> Nodes { get; set; }

        [JsonPropertyName("chain")]
        public IList<BlockInfo> Chain { get; set; }

        [JsonPropertyName("pending-transactions")]
        public ConcurrentQueue<TransactionInfo> Transactions { get; set; }

        [JsonPropertyName("is-valid")]
        public bool IsValid
        {
            get
            {
                for (var i = 1; i < Chain.Count; i++) {
                    var previous = Chain[i - 1];
                    var current = Chain[i];

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
            => Chain.Sum(x => x.Transactions.Sum(s => s.Amount));

        public event Func<BlockInfo, Task> OnBlockAdded;

        public Blockchain() {
            Nodes = new Collection<NodeInfo>();
            Chain = new Collection<BlockInfo>();
            Transactions = new ConcurrentQueue<TransactionInfo>();

            var genesis = new BlockInfo {
                PreviousHash = "0",
                Proof = 0,
                TimeStamp = DateTimeOffset.Now,
                Transactions = new Collection<TransactionInfo>()
            };

            Hashing.Create(ref genesis);
            Chain.Add(genesis);
        }

        public void AddNode(NodeInfo node) {
            Nodes.Add(node);
        }

        public void RemoveNode(NodeInfo node) {
            Nodes.Remove(node);
        }

        public void AddBlock(ref BlockInfo block) {
            block.PreviousHash = Chain[^1].Hash;
            Hashing.Create(ref block);
            Chain.Add(block);
            OnBlockAdded?.Invoke(block);
        }

        public void MineBlock(BlockInfo block, int proofOfWork) {
            var hashValidationTemplate = new string('0', proofOfWork);

            while (block.Hash.Substring(0, proofOfWork) != hashValidationTemplate)
                Hashing.Create(ref block);
        }

        public void CreateTransaction(TransactionInfo transaction) {
            Transactions.Enqueue(transaction);
            if (Transactions.Count < 2)
                return;
        }

        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }
    }
}