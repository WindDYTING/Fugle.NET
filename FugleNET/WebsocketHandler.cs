#nullable enable
using System;
using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FugleNET.Websockets;
using Python.Runtime;

namespace FugleNET
{
    public class WebsocketHandler : IDisposable, IAsyncDisposable
    {
        private readonly ILogger? _logger;
        private WebsocketsTransport _transport;
        private CancellationTokenSource _cts;

        public WebsocketHandler(ILogger? logger)
        {
            _logger = logger;
        }

        public async Task ConnectAsync(string url)
        {
            _cts = new CancellationTokenSource();
            _transport = new WebsocketsTransport(_logger);
            await _transport.StartAsync(new Uri(url));

            ThreadPool.QueueUserWorkItem(_ => RunForeverAsync().ConfigureAwait(false));
        }

        public Task DisconnectAsync()
        {
            _cts.Cancel(false);
            _cts.Dispose();
            return _transport.StopAsync();
        }

        private async Task RunForeverAsync()
        {
            var token = _cts.Token;
            while (!token.IsCancellationRequested)
            {
                var result = await _transport.Input.ReadAsync(token);
                var buffer = result.Buffer;

                if (!buffer.IsEmpty)
                {
                    OnMessage(buffer.ToArray());
                }
                else if (result.IsCompleted)
                {
                    break;
                }

                _transport.Input.AdvanceTo(buffer.End); 
            }
        }

        private void OnMessage(byte[] packets)
        {
            try
            {
                string wsMsg = Encoding.Default.GetString(packets);
                try
                {
                    var wsObject = FuglePyCore.CoreModule.convert_ws_object(wsMsg);
                    wsMsg = wsObject.As<string>();
                }
                catch
                {
                    // ignored
                }

                _logger.Debug(wsMsg);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
        }

        public void Dispose()
        {
            if (_transport is IDisposable transportDisposable)
                transportDisposable.Dispose();
            else
                _ = _transport.DisposeAsync().AsTask();
        }

        public async ValueTask DisposeAsync()
        {
            await _transport.DisposeAsync();
        }
    }
}
