using System.Threading.Tasks;
using Breadloaf.Logging;
using Breadloaf.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Breadloaf {
    public readonly struct Program {
        public static async Task Main(string[] args) {
            Extensions.PrintHeaderAndInformation();
            await Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup(typeof(Startup)))
                .ConfigureLogging((hostBuilder, logging) => {
                    logging.ClearProviders();
                    logging.AddProvider(new LoggerProvider());
                })
                .Build()
                .RunAsync()
                .ConfigureAwait(false);
        }
    }
}