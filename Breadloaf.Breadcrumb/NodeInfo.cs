﻿using System.Net.WebSockets;

namespace Breadloaf.Breadcrumb {
    public readonly struct NodeInfo {
        public string Address { get; }
        public WebSocket Socket { get; }

        public NodeInfo(string address, WebSocket socket) {
            Address = address;
            Socket = socket;
        }
    }
}