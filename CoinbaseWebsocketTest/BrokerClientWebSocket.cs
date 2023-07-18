using System.Net.WebSockets;

namespace CoinbaseWebsocketTest
{
    class BrokerClientWebSocket
    {
        private readonly ClientWebSocket WebSocket;
        private string Uri { get; set; }
        private Action<string> OnMessageReceived { get; set; }

        public BrokerClientWebSocket(string uri, Action<string> onMessageReceived)
        {
            WebSocket = new ClientWebSocket();
            Uri = uri;
            OnMessageReceived = onMessageReceived;

            StartLoop();
        }

        public void Connect()
        {
            if (WebSocket.State != WebSocketState.Open)
            {
                WebSocket.ConnectAsync(new Uri(Uri), CancellationToken.None).Wait();
            }
        }

        readonly byte[] Buffer = new byte[1000000];
        public void StartLoop()
        {
            Task.Factory.StartNew(() =>
            {
                while (WebSocket.State != WebSocketState.Closed)
                {
                    if (WebSocket.State == WebSocketState.Open)
                    {
                        var result = WebSocket.ReceiveAsync(new ArraySegment<byte>(Buffer), CancellationToken.None).Result;
                        switch (result.MessageType)
                        {
                            case WebSocketMessageType.Text:
                                string message = System.Text.Encoding.UTF8.GetString(Buffer, 0, result.Count);
                                if (!string.IsNullOrEmpty(message))
                                {
                                    Console.WriteLine($"webSocketMsg: {message}");
                                    OnMessageReceived?.Invoke(message);
                                }
                                break;
                            case WebSocketMessageType.Close:
                                WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
                                Console.WriteLine("WebSocket closed");
                                break;
                        }
                    }
                    else
                    {
                        // Sleep is required, else we pull information too quickly while the socket is still opening
                        Thread.Sleep(250);
                    }
                }
            });
        }

        public void Send(string message)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            WebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
        }
    }
}
