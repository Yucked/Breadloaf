using System;
using System.Drawing;
using Breadloaf.Utils;
using Colorful;
using Microsoft.Extensions.Logging;

namespace Breadloaf.Logging {
    public readonly struct ColorLogger : ILogger {
        private readonly string _categoryName;
        private readonly OffloadLogger _offloadLogger;

        public ColorLogger(string categoryName, OffloadLogger offloadLogger) {
            _categoryName = categoryName;
            _offloadLogger = offloadLogger;
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
            
            var formatters = new[] {
                new Formatter($"{date:MMM d - hh:mm:ss tt}", Color.Gray),
                new Formatter(Enum.GetName(typeof(LogLevel), logLevel).PadBoth(15), color),
                new Formatter(_categoryName, Color.Gold),
                new Formatter(message, Color.White)
            };

            _offloadLogger.OnMessage?.Invoke(formatters);
        }
    }
}