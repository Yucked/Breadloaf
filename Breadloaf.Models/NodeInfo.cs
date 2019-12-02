using System.Net;
using System.Net.WebSockets;

namespace Breadloaf.Models {
    public struct NodeInfo {
        public IPEndPoint Address { get; set; }
        public WebSocket Socket { get; set; }
    }
}