namespace FugleNET.Models
{
    public class WebsocketAck
    {
        public string Event { get; set; }

        public string? Data { get; set; }

        public string? Id { get; set; }

        public string? Channel { get; set; }
    }
}
