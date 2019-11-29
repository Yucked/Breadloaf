using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace Breadloaf.Breadcrumb {
    public sealed class Blockchain {
        public IList<NodeInfo> Nodes { get; set; }
        public IList<BlockInfo> Chain { get; set; }
        public ConcurrentQueue<TransactionInfo> Transactions { get; set; }

        public bool IsValid
        {
            get
            {
                for (var i = 1; i < Chain.Count; i++) {
                    var previous = Chain[i - 1];
                    var current = Chain[i];

                    if (current.PreviousHash != previous.Hash)
                        return false;
                }

                return true;
            }
        }

        public double Crumbs
            => Chain.Sum(x => x.Transactions.Sum(s => s.Amount));

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
            Chain.Add(block);
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