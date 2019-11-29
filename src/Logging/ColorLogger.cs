using System;
using System.Drawing;
using System.Threading.Tasks;
using Breadloaf.Utils;
using Colorful;
using Microsoft.Extensions.Logging;
using Console = Colorful.Console;

namespace Breadloaf.Logging {
    public readonly struct ColorLogger : ILogger {
        private readonly string _categoryName;

        public ColorLogger(string categoryName) {
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                                Func<TState, Exception, string> formatter) {
            var message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message))
                return;

            var date = DateTimeOffset.Now;
            var color = logLevel.GetLogColor();

            const string logMessage = "[{0}] [{1}] [{2}]\n    {3}";
            var formatters = new[] {
                new Formatter($"{date:MMM d - hh:mm:ss tt}", Color.Gray),
                new Formatter(Enum.GetName(typeof(LogLevel), logLevel).PadBoth(15), color),
                new Formatter(_categoryName, Color.Gold),
                new Formatter(message, Color.White)
            };

            Task.Run(() => Console.WriteLineFormatted(logMessage, Color.White, formatters));
        }
    }
}