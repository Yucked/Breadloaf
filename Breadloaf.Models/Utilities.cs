﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Breadloaf.Models {
    public static class Utilities {
        private static readonly Random Random = new Random();

        private static double NextDouble
            => Random.NextDouble() * (0.001 - 1.0) + 1.0;

        public static TransactionInfo DummyTransaction
            => new TransactionInfo {
                Memo = "Dummy Transaction",
                Amount = NextDouble,
                From = GenerateRandomAddress(),
                To = GenerateRandomAddress(),
                Timestamp = DateTimeOffset.Now
            };

        public static IEnumerable<TransactionInfo> DummyTransactions
        {
            get
            {
                var maxCount = Random.Next(1, 5);
                for (var i = 0; i < maxCount; i++) {
                    yield return new TransactionInfo {
                        Memo = $"Dummy Transaction #${i}",
                        Amount = NextDouble,
                        From = GenerateRandomAddress(),
                        To = GenerateRandomAddress(),
                        Timestamp = DateTimeOffset.Now
                    };
                }
            }
        }

        private static IPEndPoint GenerateRandomAddress() {
            static int GetRandomPort() {
                return Random.Next(IPEndPoint.MinPort, IPEndPoint.MaxPort);
            }

            static string GetRandomAddress() {
                return $"{Random.Next(0, 256)}.{Random.Next(0, 255)}.{Random.Next(0, 255)}.{Random.Next(0, 255)}";
            }

            return new IPEndPoint(IPAddress.Parse(GetRandomAddress()), GetRandomPort());
        }

        public static IEnumerable<TransactionInfo> Range(int max) {
            for (var i = 0; i < max; i++)
                yield return DummyTransaction;
        }
    }
}