using System;
using System.Linq;
using Breadloaf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Breadloaf.Tests {
    [TestClass]
    public sealed class BlockchainTests {
        private readonly Blockchain _blockchain;

        public BlockchainTests() {
            _blockchain = new Blockchain();
            _blockchain.CreateGenesisBlock();
        }

        [TestMethod]
        public void CreateDummyChain() {
            for (var i = 0; i < 5; i++) {
                var block = new BlockInfo {
                    TimeStamp = DateTimeOffset.Now,
                    PreviousHash = _blockchain.Blocks[^1].Hash,
                    Transactions = Utilities.DummyTransactions.ToArray()
                };

                Hashing.Create(ref block);
                _blockchain.AddBlock(block);
            }

            Assert.IsTrue(_blockchain.IsValid);
            Assert.IsTrue(_blockchain.Crumbs > 0);
            Assert.IsTrue(_blockchain.Blocks.Count != 0);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        public void DifficultTesting(int difficulty) {
            _blockchain.Dispose();
            _blockchain.CreateGenesisBlock();

            _blockchain.Difficulty = difficulty;
            _blockchain.TransactionThreshold = 2;
            _blockchain.CreateTransaction(Utilities.DummyTransaction);
            _blockchain.CreateTransaction(Utilities.DummyTransaction);

            Assert.IsTrue(_blockchain.IsValid);
            Assert.IsTrue(_blockchain.Crumbs > 0);
            Assert.IsTrue(_blockchain.Blocks.Count == 2);
        }
    }
}