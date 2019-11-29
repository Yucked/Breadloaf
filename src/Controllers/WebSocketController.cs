using System;
using System.Text.Json;
using System.Threading.Tasks;
using Breadloaf.Infos;
using Microsoft.Extensions.Logging;

namespace Breadloaf.Controllers {
    public sealed class WebSocketController : BaseWebSocketController {
        private readonly ILogger<WebSocketController> _logger;
        private bool _isSynced;

        public WebSocketController(Blockchain blockchain, ILogger<WebSocketController> logger)
            : base(blockchain, logger) {
            _logger = logger;
        }

        public override async Task ReceiveAsync(NodeInfo node, ReadOnlyMemory<byte> buffer) {
            if (buffer.IsEmpty)
                return;

            var blockchain = JsonSerializer.Deserialize<Blockchain>(buffer.Span);
            if (blockchain.IsValid && blockchain.Chain.Count > Blockchain.Chain.Count) { }

            if (!_isSynced) {
                await SendMessageAsync(node.Socket, JsonSerializer.Serialize(Blockchain));
                _isSynced = true;
            }

            await base.ReceiveAsync(node, buffer);
        }
    }
}