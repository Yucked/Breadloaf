using Microsoft.Extensions.Logging;

namespace Breadloaf.Logging {
    public sealed class LoggerProvider : ILoggerProvider {
        private readonly OffloadLogger _offloadLogger;

        public LoggerProvider() {
            _offloadLogger = new OffloadLogger();
            _offloadLogger.OnMessage += _offloadLogger.OnLogMessageAsync;
        }

        public void Dispose() { }

        public ILogger CreateLogger(string categoryName) {
            return new ColorLogger(categoryName, _offloadLogger);
        }
    }
}