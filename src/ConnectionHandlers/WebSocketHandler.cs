using System.Net;
using System.Threading.Tasks;
using Breadloaf.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Logging;

namespace Breadloaf.ConnectionHandlers {
    public sealed class WebSocketHandler : ConnectionHandler {
        private readonly Blockchain _blockchain;
        private readonly ILogger _logger;

        public WebSocketHandler(Blockchain blockchain, ILogger<WebSocketHandler> logger) {
            _blockchain = blockchain;
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection) {
            var context = connection.GetHttpContext();

            if (context == null || !context.WebSockets.IsWebSocketRequest) {
                connection.Abort();
                return;
            }

            var websocket = await context.WebSockets.AcceptWebSocketAsync()
                .ConfigureAwait(false);

            var node = new NodeInfo {
                Address = new IPEndPoint(context.Connection.RemoteIpAddress, context.Connection.RemotePort),
                Socket = websocket
            };

            _blockchain.AddNode(node);
            _logger.LogInformation($"Added node with {node.Address} address.");
            
            node.ReceiveAsync()
                .ConfigureAwait(false);
            
            _logger.LogInformation($"Receiving data from {node.Address} address.");
        }
    }
}