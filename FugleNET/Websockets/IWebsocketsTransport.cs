using System.IO.Pipelines;

namespace FugleNET.Websockets
{
    public interface IWebsocketsTransport
    {
        PipeOptions SocketPipeOptions { get; set; }

        PipeReader Input { get; }

        PipeWriter Output { get; }
    }
}