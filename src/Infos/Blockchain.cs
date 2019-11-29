using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using Breadloaf.Utils;
using Microsoft.Extensions.Logging;

namespace Breadloaf.Infos {
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

        public double Balance
            => Chain.Sum(x => x.Transactions.Sum(s => s.Amount));

        private readonly ILogger _logger;

        public Blockchain(ILogger<Blockchain> logger) {
            _logger = logger;
            Nodes = new Collection<NodeInfo>();
            Chain = new Collection<BlockInfo>();
            Transactions = new ConcurrentQueue<TransactionInfo>();

            var genesis = new BlockInfo {
                PreviousHash = "0",
                Proof = 0,
                TimeStamp = DateTimeOffset.Now,
                Transactions = new Collection<TransactionInfo>()
            };

            Extensions.CreateHash(ref genesis);
            Chain.Add(genesis);

            _logger.LogInformation($"Created genesis block:\n{genesis}");
        }

        public void AddNode(NodeInfo node) {
            Nodes.Add(node);
            _logger.LogInformation($"Node connected: {node}");
        }

        public void RemoveNode(NodeInfo node) {
            Nodes.Remove(node);
            _logger.LogInformation($"Node disconnected: {node}");
        }

        public void AddBlock(ref BlockInfo block) {
            block.PreviousHash = Chain[^1].Hash;
            Chain.Add(block);
        }

        public void MineBlock(BlockInfo block) {
            _logger.LogInformation($"Block mined:\n{block}");
        }

        public void CreateTransaction(TransactionInfo transaction) {
            Transactions.Enqueue(transaction);
            if (Transactions.Count < 2)
                return;

            _logger.LogInformation("Beginning block mining");
        }

        public override string ToString() {
            return JsonSerializer.Serialize(this);
        }
    }
}