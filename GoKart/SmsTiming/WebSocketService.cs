using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace GoKart.SmsTiming
{
    class WebSocketService
    {
        Task task;

        public WebSocketService(string url, string liveServerKey, OnJSONReceived OnJSONReceived, CancellationToken cancellationToken = default(CancellationToken))
        {
            task = WebSocketServiceAsync(url, liveServerKey, OnJSONReceived, cancellationToken);
        }

        private async Task WebSocketServiceAsync(string url, string liveServerKey, OnJSONReceived OnJSONReceived, CancellationToken cancellationToken = default(CancellationToken))
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var ClientWebSocket = new ClientWebSocket())
                {
                    try
                    {
                        await ClientWebSocket.ConnectAsync(new Uri(url), cancellationToken);
                        await SendAsync(ClientWebSocket, "START " + liveServerKey, cancellationToken);
                        await ReceiveAsync(ClientWebSocket, OnJSONReceived, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WebSocketServiceAsync: {ex.Message}");
                    }
                }
            }
        }

        private async Task SendAsync(ClientWebSocket socket, string data, CancellationToken cancellationToken = default(CancellationToken))
        {
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, cancellationToken);
        }

        private async Task ReceiveAsync(ClientWebSocket socket, OnJSONReceived OnJSONReceived, CancellationToken cancellationToken = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);

            while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;

                using (var memoryStream = new MemoryStream())
                {
                    try
                    {
                        do
                        {
                            result = await socket.ReceiveAsync(buffer, cancellationToken);
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                            }
                        } while (!result.EndOfMessage && !cancellationToken.IsCancellationRequested);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine(result.CloseStatusDescription);
                            break;
                        }

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                        {
                            OnJSONReceived?.Invoke(reader.ReadToEndAsync().Result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WebSocketServiceAsync:ReceiveAsync: {ex.Message}");
                    }
                }
            };
        }
    }
}