using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FugleNET.Websockets
{
    internal class WebsocketsTransport : IAsyncDisposable
    {
        private readonly ILogger _logger;
        private const int DefaultBufferSize = 1 << 20;
        private const int DefaultCloseTimeout = 5000;
        private readonly CancellationTokenSource _cts;
        private IDuplexPipe _transport;
        private IDuplexPipe _application;
        private ClientWebSocket _socket;

        internal Task Running { get; set; } = Task.CompletedTask;

        public PipeOptions SocketPipeOptions { get; set; } = new(
            pauseWriterThreshold: DefaultBufferSize,
            resumeWriterThreshold: DefaultBufferSize / 2,
            readerScheduler: PipeScheduler.ThreadPool,
            useSynchronizationContext: false);

        // public Task ProcessMessageTask { get; set; } = Task.CompletedTask;

        public PipeReader Input => _transport.Input;

        public PipeWriter Output => _transport.Output;

        public WebsocketsTransport(ILogger logger)
        {
            _logger = logger;
            _cts = new CancellationTokenSource();
        }

        public async Task SendAsync(string msg)
        {
            if (!WebsocketCanBeSend(_socket))
            {
                _logger.WebsocketCannotSent();
                return;
            }

            var messageBytes = Encoding.UTF8.GetBytes(msg);
            _transport.Output.Write(messageBytes);
            await _transport.Output.FlushAsync().ConfigureAwait(false);
        }

        public void Send(string msg)
        {
            if (!WebsocketCanBeSend(_socket))
            {
                _logger.WebsocketCannotSent();
                return;
            }
            SendAsync(msg).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task StartAsync(Uri serverUri)
        {
            _socket = new ClientWebSocket();
            await _socket.ConnectAsync(serverUri, CancellationToken.None).ConfigureAwait(false);

            if (_transport is null)
            {
                var pair = DuplexPipe.CreateConnectionPair(SocketPipeOptions, SocketPipeOptions);
                _transport = pair.Transport;
                _application = pair.Application;
            }

            Running = ProcessWebsocketAsync(_socket, serverUri);
            //ProcessMessageTask = ProcessMessageAsync();
        }

        private async Task ProcessMessageAsync()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var result = await _transport.Input.ReadAsync().ConfigureAwait(false);
                    var buffer = result.Buffer;
                    if (buffer.IsEmpty && result.IsCompleted)
                    {
                        break;
                    }

                    foreach (var segment in buffer)
                    {
                        var msg = Encoding.UTF8.GetString(segment.Span);
                        try
                        {
                            //OnMessage?.Invoke(msg);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e.ToString());
                        }
                    }

                    _transport.Input.AdvanceTo(buffer.End);
                }

                _transport.Input.Complete();
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                throw;
            }
        }

        public async Task StopAsync()
        {
            if (_application == null)
            {
                return;
            }

            _transport.Output.Complete();
            _transport.Input.Complete();
            _application.Input.CancelPendingRead();

            _cts.CancelAfter(DefaultCloseTimeout);

            try
            {
                await Running.ConfigureAwait(false);
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _socket.Dispose();
                _cts.Dispose();
            }
        }

        private bool UpdateConnectionPair()
        {
            lock (this)
            {

                var input = new Pipe(SocketPipeOptions);

                var transportToApplication = new DuplexPipe(_transport.Input, input.Writer);
                var applicationToTransport = new DuplexPipe(input.Reader, _application.Output);

                _application = applicationToTransport;
                _transport = transportToApplication;
            }

            return true;
        }

        private async Task ProcessWebsocketAsync(WebSocket ws, Uri serverUri)
        {
            var receiveTask = StartReceiveAsync(ws, _application.Output);
            var sendingTask = StartSendingAsync(ws, _application.Input);

            var trigger = await Task.WhenAny(receiveTask, sendingTask);
            if (trigger == receiveTask)
            {
                _application.Input.CancelPendingRead();
                using (var delayCts = new CancellationTokenSource())
                {
                    var resultTask = await Task.WhenAny(sendingTask, Task.Delay(DefaultCloseTimeout, delayCts.Token));
                    if (resultTask != sendingTask)
                    {
                        ws.Abort();
                    }
                    else
                    {
                        delayCts.Cancel();
                    }
                }
            }
            else
            {
                using (var delayCts = new CancellationTokenSource())
                {
                    var resultTask = await Task.WhenAny(receiveTask, Task.Delay(DefaultCloseTimeout, delayCts.Token));
                    if (resultTask != receiveTask)
                    {
                        ws.Abort();

                        _application.Output.CancelPendingFlush();
                    }
                    else
                    {
                        delayCts.Cancel();
                    }
                }
            }

            var cleanup = true;
            try
            {
                if (UpdateConnectionPair())
                {
                    try
                    {
                        await StartAsync(serverUri).ConfigureAwait(false);
                        cleanup = false;
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException("Reconnect attempt failed.", innerException: e);
                    }
                }
            }
            finally
            {
                if (cleanup)
                {
                    _application.Output.Complete();
                    _application.Input.Complete();
                }
            }
        }

        #nullable enable
        private async Task StartSendingAsync(WebSocket ws, PipeReader reader)
        {
            Exception? error = null;
            try
            {
                while (true)
                {
                    var result = await reader.ReadAsync().ConfigureAwait(false);
                    var buffer = result.Buffer;

                    try
                    {
                        if (result.IsCanceled)
                        {
                            break;
                        }


                        if (!buffer.IsEmpty)
                        {
                            try
                            {
                                foreach (var segment in buffer)
                                {
                                    if (WebsocketCanBeSend(ws))
                                    {
                                        await ws.SendAsync(segment, WebSocketMessageType.Binary, true,
                                            CancellationToken.None).ConfigureAwait(false);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                        else if (result.IsCompleted)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        reader.AdvanceTo(buffer.End);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                if (WebsocketCanBeSend(ws))
                {
                    try
                    {
                        await ws.CloseOutputAsync(error != null
                            ? WebSocketCloseStatus.InternalServerError
                            : WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.ToString());
                    }
                }
                reader.Complete();

                if (error is not null)
                {
                    _logger.Error(error.ToString());
                }
            }
        }
        #nullable disable

        private async Task StartReceiveAsync(WebSocket ws, PipeWriter writer)
        {
            var token = _cts.Token;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var result = await ws.ReceiveAsync(Memory<byte>.Empty, token).ConfigureAwait(false);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return;
                    }

                    var memory = writer.GetMemory();
                    var receivedResult = await ws.ReceiveAsync(memory, token).ConfigureAwait(false);
                    if (receivedResult.MessageType == WebSocketMessageType.Close)
                    {
                        return;
                    }

                    writer.Advance(receivedResult.Count);
                    var flushResult = await writer.FlushAsync().ConfigureAwait(false);
                    if (flushResult.IsCanceled || flushResult.IsCompleted)
                    {
                        break;
                    }
                }
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                _logger.Error(ex.ToString());
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                if (!token.IsCancellationRequested)
                {
                    writer.Complete(ex);
                }
            }
            finally
            {
                writer.Complete();
            }
        }

        private bool WebsocketCanBeSend(WebSocket ws)
        {
            return ws.State is not (WebSocketState.Aborted or WebSocketState.Closed or WebSocketState.CloseSent);
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();

            await CastAndDispose(_cts);
            await CastAndDispose(_socket);
            await CastAndDispose(Running);
            //await CastAndDispose(ProcessMessageTask);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync();
                else
                    resource.Dispose();
            }
        }
    }
}
