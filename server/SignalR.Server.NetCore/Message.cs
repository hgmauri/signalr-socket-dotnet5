namespace SignalR.Hub.Server
{
    public class Message
    {
        public string Type { get; set; }
        public string Client { get; set; }
        public string Payload { get; set; }
    }
}