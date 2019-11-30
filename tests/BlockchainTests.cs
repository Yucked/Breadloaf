using System;
using System.Linq;
using Breadloaf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Breadloaf.Tests {
    [TestClass]
    public sealed class BlockchainTests {
        private readonly Blockchain _blockchain
            = new Blockchain();

        [TestMethod]
        public void CreateDummyChain() {
            for (var i = 0; i < 1000; i++) {
                var block = new BlockInfo {
                    TimeStamp = DateTimeOffset.Now,
                    PreviousHash = "000000",
                    Transactions = Utilities.DummyTransactions.ToArray()
                };

                Hashing.Create(ref block);
                _blockchain.AddBlock(ref block);
            }
            
            Assert.IsTrue(_blockchain.IsValid);
            Assert.IsTrue(_blockchain.Crumbs > 0);
            Assert.IsTrue(_blockchain.Chain.Count != 0);
        }
    }
}