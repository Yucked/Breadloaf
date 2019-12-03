using System;
using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Breadloaf.Models {
    public struct NodeInfo {
        public IPEndPoint Address { get; set; }
        public WebSocket Socket { get; set; }

        public event Func<NodeInfo, Task> OnClosed;
        public event Func<NodeInfo, ReadOnlyMemory<byte>, Task> OnMessage;

        public Task SendAsync(object data) {
            return Socket.SendAsync(JsonSerializer.SerializeToUtf8Bytes(data), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }

        public async Task ReceiveAsync() {
            try {
                while (Socket.State == WebSocketState.Open) {
                    var buffer = new byte[512];
                    var result = await Socket.ReceiveAsync(buffer, CancellationToken.None);

                    switch (result.MessageType) {
                        case WebSocketMessageType.Text:
                            if (!result.EndOfMessage)
                                continue;

                            var lastIndex = Array.FindLastIndex(buffer, b => b != 0);
                            Array.Resize(ref buffer, lastIndex + 1);
                            OnMessage?.Invoke(this, buffer);
                            continue;

                        case WebSocketMessageType.Close:
                            OnClosed?.Invoke(this);
                            continue;
                    }
                }
            }
            catch {
                OnClosed?.Invoke(this);
            }
        }
    }
}