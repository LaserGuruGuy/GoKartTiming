using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoKart.SmsTiming
{
    class WebSocketService
    {
        protected OnJSONReceived OnJSONReceived = null;

        public WebSocketService(string url, string liveServerKey, OnJSONReceived OnJSONReceived = null)
        {
            this.OnJSONReceived = OnJSONReceived;
            Task task = WebSocketServiceAsync(url, liveServerKey, CancellationToken.None);
        }

        private async Task WebSocketServiceAsync(string url, string liveServerKey, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var ClientWebSocket = new ClientWebSocket())
                {
                    try
                    {
                        await ClientWebSocket.ConnectAsync(new Uri(url), stoppingToken);
                        await SendAsync(ClientWebSocket, "START " + liveServerKey, stoppingToken);
                        await ReceiveAsync(ClientWebSocket, stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR - {ex.Message}");
                    }
                }
            }
        }

        private async Task SendAsync(ClientWebSocket socket, string data, CancellationToken stoppingToken)
        {
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, stoppingToken);
        }

        private async Task ReceiveAsync(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using (var memoryStream = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, stoppingToken);
                        memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close) break;

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(memoryStream, System.Text.Encoding.UTF8))
                    {
                        OnJSONReceived?.Invoke(JsonConvert.SerializeObject(reader.ReadToEndAsync().Result));
                    }
                }
            };
        }
    }
}