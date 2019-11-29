using System;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Breadloaf.Infos;
using Colorful;
using Microsoft.Extensions.Logging;
using Console = Colorful.Console;

namespace Breadloaf.Utils {
    public static class Extensions {
        private static readonly Random Random = new Random();

        public static double NextDouble
            => Random.NextDouble() * (700 - 100) + 100;

        public static Color GetLogColor(this LogLevel logLevel) {
            return logLevel switch {
                LogLevel.Information => Color.SpringGreen,
                LogLevel.Debug       => Color.Coral,
                LogLevel.Trace       => Color.MediumPurple,
                LogLevel.Critical    => Color.Red,
                LogLevel.Error       => Color.Crimson,
                LogLevel.Warning     => Color.Orange,
                _                    => Color.Tomato
            };
        }

        public static string PadBoth(this string source, int length) {
            var spaces = length - source.Length;
            var padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft).PadRight(length);
        }

        public static void PrintHeaderAndInformation() {
            const string logo =
                @"
                    ▄▄▄▄    ██▀███  ▓█████ ▄▄▄      ▓█████▄  ██▓     ▒█████   ▄▄▄        █████▒
                    ▓█████▄ ▓██ ▒ ██▒▓█   ▀▒████▄    ▒██▀ ██▌▓██▒    ▒██▒  ██▒▒████▄    ▓██   ▒ 
                    ▒██▒ ▄██▓██ ░▄█ ▒▒███  ▒██  ▀█▄  ░██   █▌▒██░    ▒██░  ██▒▒██  ▀█▄  ▒████ ░ 
                    ▒██░█▀  ▒██▀▀█▄  ▒▓█  ▄░██▄▄▄▄██ ░▓█▄   ▌▒██░    ▒██   ██░░██▄▄▄▄██ ░▓█▒  ░ 
                    ░▓█  ▀█▓░██▓ ▒██▒░▒████▒▓█   ▓██▒░▒████▓ ░██████▒░ ████▓▒░ ▓█   ▓██▒░▒█░    
                    ░▒▓███▀▒░ ▒▓ ░▒▓░░░ ▒░ ░▒▒   ▓▒█░ ▒▒▓  ▒ ░ ▒░▓  ░░ ▒░▒░▒░  ▒▒   ▓▒█░ ▒ ░    
                    ▒░▒   ░   ░▒ ░ ▒░ ░ ░  ░ ▒   ▒▒ ░ ░ ▒  ▒ ░ ░ ▒  ░  ░ ▒ ▒░   ▒   ▒▒ ░ ░      
                     ░    ░   ░░   ░    ░    ░   ▒    ░ ░  ░   ░ ░   ░ ░ ░ ▒    ░   ▒    ░ ░    
                     ░         ░        ░  ░     ░  ░   ░        ░  ░    ░ ░        ░  ░        
                          ░                           ░                                         
";

            var lineBreak = new string('-', 120);
            Console.WriteLine(logo, Color.GreenYellow);
            Console.WriteLine(lineBreak);

            const string logMessage = "    Framework: {0} - OS Arch: {1} - Process Arch: {2} - OS: {3}";
            var formatters = new[] {
                new Formatter(RuntimeInformation.FrameworkDescription, Color.Aqua),
                new Formatter(RuntimeInformation.OSArchitecture, Color.Gold),
                new Formatter(RuntimeInformation.ProcessArchitecture, Color.LawnGreen),
                new Formatter(RuntimeInformation.OSDescription, Color.HotPink)
            };

            Console.WriteLineFormatted(logMessage, Color.White, formatters);
            Console.WriteLine(lineBreak);
        }

        public static void CreateHash(ref BlockInfo blockInfo) {
            var rawData = $"{blockInfo.PreviousHash}-{blockInfo.TimeStamp.Ticks}-{blockInfo.Transactions.Count}";
            using var crypto = SHA512.Create();
            var hash = crypto.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var result = new StringBuilder();
            foreach (var data in hash) result.Append($"{data:X2}");

            blockInfo.Hash = $"{result}";
            result.Clear();
        }

        public static string GetHash(this BlockInfo blockInfo) {
            var rawData = $"{blockInfo.PreviousHash}-{blockInfo.TimeStamp.Ticks}-{blockInfo.Transactions.Count}";
            using var crypto = SHA512.Create();
            var hash = crypto.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var builder = new StringBuilder();
            foreach (var data in hash) builder.Append($"{data:X2}");

            return $"{builder}";
        }

        public static void MineBlock(this BlockInfo blockInfo, int proofOfWork) {
            var hashValidationTemplate = new string('0', proofOfWork);

            while (blockInfo.Hash.Substring(0, proofOfWork) != hashValidationTemplate) CreateHash(ref blockInfo);
        }

        public static IPEndPoint GenerateRandomAddress() {
            static int GetRandomPort() {
                return Random.Next(IPEndPoint.MinPort, IPEndPoint.MaxPort);
            }

            static string GetRandomAddress() {
                return $"{Random.Next(0, 256)}.{Random.Next(0, 255)}.{Random.Next(0, 255)}.{Random.Next(0, 255)}";
            }

            return new IPEndPoint(IPAddress.Parse(GetRandomAddress()), GetRandomPort());
        }
    }
}