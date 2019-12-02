using System;
using System.Linq;
using Breadloaf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Breadloaf.Tests {
    [TestClass]
    public sealed class HashingTests {
        [TestMethod]
        public void HashEmptyBlock() {
            Assert.ThrowsException<NullReferenceException>(() => {
                var block = new BlockInfo();
                Hashing.Create(ref block);
            });
        }

        [TestMethod]
        public void HashBlock() {
            var block = new BlockInfo {
                Transactions = Utilities.DummyTransactions.ToArray(),
                TimeStamp = DateTimeOffset.Now,
                PreviousHash = "000000"
            };

            Hashing.Create(ref block);
            Assert.IsNotNull(block.Hash);

            var getHash = Hashing.Get(block);
            Assert.AreEqual(block.Hash, getHash);
        }
    }
}