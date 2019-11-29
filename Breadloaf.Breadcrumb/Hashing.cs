using System.Security.Cryptography;
using System.Text;

namespace Breadloaf.Breadcrumb {
    public struct Hashing {
        public static void Create(ref BlockInfo blockInfo) {
            var rawData = $"{blockInfo.PreviousHash}-{blockInfo.TimeStamp.Ticks}-{blockInfo.Transactions.Count}";
            using var crypto = SHA512.Create();
            var hash = crypto.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var result = new StringBuilder();
            foreach (var data in hash) result.Append($"{data:X2}");

            blockInfo.SetHash($"{result}");
            result.Clear();
        }

        public static string Get(BlockInfo blockInfo) {
            var rawData = $"{blockInfo.PreviousHash}-{blockInfo.TimeStamp.Ticks}-{blockInfo.Transactions.Count}";
            using var crypto = SHA512.Create();
            var hash = crypto.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var builder = new StringBuilder();
            foreach (var data in hash) builder.Append($"{data:X2}");

            return $"{builder}";
        }
    }
}