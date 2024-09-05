#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FugleNET.Logging;
using FugleNET.Models;
using FugleNET.Websockets;

namespace FugleNET
{
    public class FugleWebsocket : IAsyncDisposable
    {
        private readonly ILogger _logger;
        private const string ServerUrl = "wss://api.fugle.tw/marketdata/v1.0/stock/streaming";
        private readonly WebsocketsTransport _transport;
        private readonly TaskCompletionSource _waitAuthenticated;

        private FugleWebsocket(ILogger logger)
        {
            _logger = logger;
            _waitAuthenticated = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            _transport = new WebsocketsTransport(logger);
        }

        private void OnMessage(string message)
        {
            try
            {
                var ack = message.FromJson<WebsocketAck>();
                if (ack.Event == EventType.Authenticated)
                {
                    OnAuthenticated(ack);
                }
                else if (ack.Event == EventType.Error)
                {
                    OnError(ack);
                } 
                else if (ack.Event == EventType.Heartbeat)
                {
                    OnHeartbeat(ack);
                }
                else if (ack.Event == EventType.Subscribed)
                {

                } 
                else if (ack.Event == EventType.Unsubscribed)
                {

                }
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }

        private void OnError(WebsocketAck ack)
        {
            var data = ack.Data.FromJson<IReadOnlyDictionary<string, string>>();
            _logger.Error(data!["message"]);
            _waitAuthenticated.TrySetException(new Exception(data["message"]));
        }

        private void OnHeartbeat(WebsocketAck ack)
        {
#if DEBUG
            var data = ack.Data.FromJson<IReadOnlyDictionary<string, string>>();
            _logger.Debug(data["time"]);
#endif
        }

        private void OnAuthenticated(WebsocketAck ack)
        {
            var data = ack.Data.FromJson<IReadOnlyDictionary<string, string>>();
            _logger.Info(data!["message"]);
            _waitAuthenticated.TrySetResult();
        }

        public static async Task<FugleWebsocket> StartAsync(string apiKey, ILogger? logger=null)
        {
            Checks.ThrowIsBlank(apiKey);

            var socket = new FugleWebsocket(logger ?? new DefaultConsoleLogger());
            await socket._transport.StartAsync(new Uri(ServerUrl))
                .ContinueWith(async _ =>
                {
                    await socket.AuthAsync(apiKey);
                    await socket._waitAuthenticated.Task;
                }, TaskContinuationOptions.ExecuteSynchronously).ConfigureAwait(false);
            return socket;
        }

        public async Task AuthAsync(string apiKey)
        {
            var json = new
            {
                @event = "auth",
                data = new
                {
                    apikey = apiKey
                }
            }.ToJson();
            await _transport.SendAsync(json).ConfigureAwait(false);
        }

        public void Auth(string apiKey)
        {
            var json = new
            {
                @event = "auth",
                data = new
                {
                    apikey = apiKey
                }
            }.ToJson();
            _transport.Send(json);
        }

        public async ValueTask DisposeAsync()
        {
            await _transport.DisposeAsync();
        }
    }
}
