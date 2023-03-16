namespace SignalRDemo.Model;

public class Message
{
    public DateTimeOffset Timestamp { get; set; }
    public string Content { get; set; } = "";
    public MessageSource Source { get; set; }
}

public enum MessageSource
{
    CHAT, USER
}