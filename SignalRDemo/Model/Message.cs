using MongoDB.Bson.Serialization.Attributes;

namespace SignalRDemo.Model;

public class Message
{
    [BsonId]
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Content { get; set; } = "";
    public MessageSource Source { get; set; }
}

public enum MessageSource
{
    CHAT, USER
}