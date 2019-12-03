using System;
using System.Drawing;
using System.Threading.Tasks;
using Colorful;
using Console = Colorful.Console;

namespace Breadloaf.Logging {
    public struct OffloadLogger {
        public Func<Formatter[], Task> OnMessage;
        private const string LOG_MESSAGE = "[{0}] [{1}] [{2}]\n    {3}";

        public async Task OnLogMessageAsync(Formatter[] formatters) {
            _ = Task.Run(() => Console.WriteLineFormatted(LOG_MESSAGE, Color.White, formatters));
        }
    }
}