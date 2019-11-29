using System;
using System.Collections.Generic;
using Breadloaf.Breadcrumb;
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
                Transactions = new List<TransactionInfo>(),
                TimeStamp = DateTimeOffset.Now,
                PreviousHash = "000000",
                Proof = 5
            };

            Hashing.Create(ref block);
            Assert.IsNotNull(block.Hash);
        }
    }
}