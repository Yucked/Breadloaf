using Microsoft.Extensions.Logging;

namespace Breadloaf.Logging {
    public readonly struct LoggerProvider : ILoggerProvider {
        public void Dispose() { }

        public ILogger CreateLogger(string categoryName) {
            return new ColorLogger(categoryName);
        }
    }
}