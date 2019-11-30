using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Breadloaf.Models;
using Microsoft.Extensions.Logging;

namespace Breadloaf.Controllers {
    public class BaseWebSocketController {
        public Blockchain Blockchain { get; }
        private readonly ILogger<BaseWebSocketController> _logger;

        protected BaseWebSocketController(Blockchain blockchain, ILogger<BaseWebSocketController> logger) {
            Blockchain = blockchain;
            _logger = logger;
        }

        public virtual Task OnConnectedAsync(NodeInfo node) {
            _logger.LogInformation($"Node with {node.Address} address connected.");
            Blockchain.AddNode(node);
            return Task.CompletedTask;
        }

        public virtual async Task OnDisconnectedAsync(NodeInfo node) {
            _logger.LogError($"Node with {node.Address} address disconnected.");

            try {
                await node.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "Closed by remote.", CancellationToken.None);
            }
            catch (Exception exception) {
                _logger.LogCritical(exception, exception.Message);
            }
            finally {
                Blockchain.RemoveNode(node);
            }
        }

        protected async Task SendMessageAsync(WebSocket socket, object data) {
            if (socket.State != WebSocketState.Open)
                return;

            var raw = JsonSerializer.SerializeToUtf8Bytes(data);
            await socket.SendAsync(raw, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendToAllAsync(object data) {
            foreach (var node in Blockchain.Nodes) {
                if (node.Socket.State != WebSocketState.Open)
                    continue;

                await SendMessageAsync(node.Socket, data);
            }
        }

        public virtual Task ReceiveAsync(NodeInfo node, ReadOnlyMemory<byte> buffer) {
            return Task.CompletedTask;
        }
    }
}