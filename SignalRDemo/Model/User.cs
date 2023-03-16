using MongoDB.Bson.Serialization.Attributes;

namespace SignalRDemo.Model;

public class User
{
    [BsonId]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public List<Message> Messages { get; set; } = new List<Message>();
}