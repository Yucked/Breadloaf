using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Breadloaf.Models;
using Breadloaf.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Breadloaf.Middlewares {
    public readonly struct WebSocketMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebSocketMiddleware> _logger;

        public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, WebSocketController controller) {
            if (!context.WebSockets.IsWebSocketRequest) {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Breadloaf"))
                return;

            var address = $"http://{context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}";
            _logger.LogInformation(
                $"Incoming websocket request from {address} address.");

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var connection = new NodeInfo(address, socket);
            await controller.OnConnectedAsync(connection);
            await ReceiveAsync(connection, controller);
        }

        private async Task ReceiveAsync(NodeInfo node, BaseWebSocketController controller) {
            try {
                while (node.Socket.State == WebSocketState.Open) {
                    var buffer = new byte[512];
                    var result = await node.Socket.ReceiveAsync(buffer, CancellationToken.None);
                    switch (result.MessageType) {
                        case WebSocketMessageType.Text:
                            if (!result.EndOfMessage)
                                continue;

                            var lastIndex = Array.FindLastIndex(buffer, b => b != 0);
                            Array.Resize(ref buffer, lastIndex + 1);

                            await controller.ReceiveAsync(node, buffer);
                            continue;

                        case WebSocketMessageType.Close:
                            _logger.LogWarning(
                                $"Client {node.Address} address disconnected.");
                            await controller.OnDisconnectedAsync(node);
                            continue;
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogCritical(ex, ex.Message);
                await controller.OnDisconnectedAsync(node);
            }
        }
    }
}