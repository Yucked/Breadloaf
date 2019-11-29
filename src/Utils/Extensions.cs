using System.Drawing;
using System.Runtime.InteropServices;
using Colorful;
using Microsoft.Extensions.Logging;
using Console = Colorful.Console;

namespace Breadloaf.Utils {
    public static class Extensions {
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
    }
}